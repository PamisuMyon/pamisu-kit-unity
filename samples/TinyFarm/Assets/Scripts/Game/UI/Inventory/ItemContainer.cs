using System.Collections.Generic;
using Game.Inventory;
using Game.Inventory.Models;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.Inventory
{
    public class ItemContainer : MonoEntity
    {
        [FormerlySerializedAs("_slots")]
        [SerializeField]
        protected ItemSlot[] InitSlots;

        [FormerlySerializedAs("_collection")]
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
            
            for (int i = 0; i < Slots.Count; i++)
            {
                if (i < items.Count)
                {
                    Slots[i].Item = items[i];
                }
                Slots[i].Refresh();
            }

            Inventory = GetSystem<InventorySystem>();
            var data = Inventory.GetContainerData(Id);
            if (data == null)
            {
                Inventory.CreateContainerData(Id);
            }
            UpdateContainerData();
        }

        private void UpdateContainerData()
        {
            var data = Data;
            data.SlotDataDict.Clear();
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Item == null)
                    continue;
                data.SlotDataDict[i] = new ItemSlotData
                {
                    Index = i,
                    ItemId = Slots[i].Item.Id
                };
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