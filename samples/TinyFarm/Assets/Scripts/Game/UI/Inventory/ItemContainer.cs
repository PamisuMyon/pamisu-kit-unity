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
        private ItemSlot[] _slots;

        [SerializeField]
        private ItemCollection _collection;

        private InventorySystem _inventorySystem;
        
        public string Id { get; private set; }
        public List<ItemSlot> Slots { get; } = new();
        public ItemContainerData Data => _inventorySystem.GetContainerData(Id);

        protected override void OnCreate()
        {
            base.OnCreate();
            Debug.Assert(_collection, "ItemCollection can't be null.");

            Id = Go.name;

            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].Index = i;
                _slots[i].Container = this;
            }
            Slots.AddRange(_slots);

            var items = _collection.Items;
            var itemCount = items.Count;
            if (itemCount > Slots.Count)
            {
                Debug.LogError($"Not enough slots, expected {itemCount}, currently {Slots.Count}", Go);
                itemCount = Slots.Count;
            }
            for (int i = 0; i < itemCount; i++)
            {
                Slots[i].Item = items[i];
                Slots[i].Refresh();
            }

            _inventorySystem = GetSystem<InventorySystem>();
            var data = _inventorySystem.GetContainerData(Id);
            if (data == null)
            {
                _inventorySystem.CreateContainerData(Id);
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
        
    }
}