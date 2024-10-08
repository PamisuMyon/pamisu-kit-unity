using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Inventory.Models;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Inventory
{
    public class ItemCollection : MonoEntity
    {
        [SerializeField]
        private CollectionInitItem[] _initItems;

        private InventorySystem _inventorySystem;
        
        public string Id { get; private set; }
        public ItemCollectionData Data => _inventorySystem.GetCollectionData(Id);
        public List<Item> Items => Data.Items;

        protected override void OnCreate()
        {
            base.OnCreate();
            Id = Go.name;
            
            _inventorySystem = GetSystem<InventorySystem>();
            var data = Data;
            if (data == null)
            {
                data = _inventorySystem.CreateCollectionData(Id);
                for (int i = 0; i < _initItems.Length; i++)
                {
                    var item = Item.Create(_initItems[i].Config, _initItems[i].Amount);
                    data.Items.Add(item);
                }
            }
        }

        public void AddItem(Item item)
        {
            
        }

        public void RemoveItem(Item item)
        {
            item.NotifyRemoving();
            Items.Remove(item);
        }

    }

    [Serializable]
    public class CollectionInitItem
    {
        public ItemConfig Config;
        public int Amount = 1;
    }
    
}