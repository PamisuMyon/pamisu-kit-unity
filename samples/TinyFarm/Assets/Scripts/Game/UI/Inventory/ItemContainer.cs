using System.Collections.Generic;
using Game.Inventory;
using Game.Inventory.Models;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI.Inventory
{
    public class ItemContainer : MonoEntity
    {
        [SerializeField]
        protected ItemSlot[] InitSlots;

        [SerializeField]
        protected ItemCollection Collection;

        protected InventorySystem Inventory;
        
        public string Id { get; private set; }
        public List<ItemSlot> Slots { get; } = new();
        public ItemContainerData Data => Inventory.GetContainerData(Id);

        protected override void OnCreate()
        {
            base.OnCreate();
            Debug.Assert(Collection, "ItemCollection can't be null.");

            Id = Go.name;

            for (int i = 0; i < InitSlots.Length; i++)
            {
                InitSlots[i].Setup(Region);
                InitSlots[i].Index = i;
                InitSlots[i].Container = this;
            }
            Slots.AddRange(InitSlots);

            var items = Collection.Items;
            if (items.Count > Slots.Count)
                Debug.LogError($"Not enough slots, expected {items.Count}, currently {Slots.Count}", Go);
            // TODO auto generate slots
            
            Inventory = GetSystem<InventorySystem>();
            var data = Inventory.GetContainerData(Id);
            if (data == null)
            {
                data = Inventory.CreateContainerData(Id);
                for (int i = 0; i < Slots.Count; i++)
                {
                    if (i < items.Count)
                    {
                        Slots[i].Item = items[i];
                    }
                }
            }
            else
            {
                for (int i = 0, j = 0; i < items.Count; i++)
                {
                    // Put item in the corresponding slot if it's recorded in the dict
                    if (data.ItemSlotDict.TryGetValue(items[i].Id, out int index)
                        && index < Slots.Count)
                    {
                        Slots[index].Item = items[i];
                    }
                    else
                    {
                        // otherwise put it in a spare slot
                        Debug.Log($"Item {items[i]} slot index not recorded, finding a spare slot...", Go);
                        for (; j < Slots.Count && data.ItemSlotDict.ContainsValue(j); j++)
                        {
                        }
                        if (j >= Slots.Count)
                        {
                            Debug.LogError($"No spare slot found for item {items[i]}", Go);
                            break;
                        }
                        Slots[j].Item = items[i];
                        j++;
                    }
                }
            }
            UpdateContainerData();
            
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Refresh();
                Slots[i].Changed += OnSlotChanged;
            }
        }

        private void OnSlotChanged(ItemSlot slot, Item oldItem, Item newItem)
        {
            var data = Data;
            if (oldItem != null)
            {
                data.ItemSlotDict.Remove(oldItem.Id);
            }
            if (newItem != null)
            {
                data.ItemSlotDict[newItem.Id] = slot.Index;
            }
        }

        private void UpdateContainerData()
        {
            var data = Data;
            data.ItemSlotDict.Clear();
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Item == null)
                    continue;
                data.ItemSlotDict[Slots[i].Item.Id] = i;
            }
        }

        public void RemoveItem(int index)
        {
            var slot = Slots[index];
            Collection.RemoveItem(slot.Item);
        }

        public void StackOrSwap(ItemSlot from, ItemSlot to)
        {
            if (from == to)
                return;
            
            if (from.Item != null && to.Item != null
                && from.Item.Config == to.Item.Config)
            {
                // TODO the max stack amount has not yet been considered
                to.Item.ChangeAmount(from.Item.Amount);
                RemoveItem(from.Index);
            }
            else
            {
                (to.Item, from.Item) = (from.Item, to.Item);
                from.Refresh();
                to.Refresh();
            }
        }
        
    }
}