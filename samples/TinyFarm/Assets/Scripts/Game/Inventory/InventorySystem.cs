using System.Collections.Generic;
using Game.Configs;
using Game.Inventory.Models;
using Game.Save;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Inventory
{
    public class InventorySystem : MonoSystem
    {

        [SerializeField]
        private ItemCollection[] _collections;

        private SaveSystem _saveSystem;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _saveSystem = GetSystem<SaveSystem>();

            for (int i = 0; i < _collections.Length; i++)
            {
                _collections[i].Setup(Region);
            }
        }

        public ItemCollectionData GetCollectionData(string id)
        {
            return _saveSystem.SaveData.Inventory.CollectionDataDict.GetValueOrDefault(id);
        }

        public ItemCollectionData CreateCollectionData(string id)
        {
            var data = new ItemCollectionData
            {
                Id = id,
                Items = new List<Item>(),
            };
            _saveSystem.SaveData.Inventory.CollectionDataDict[id] = data;
            return data;
        }

        public ItemContainerData GetContainerData(string id)
        {
            return _saveSystem.SaveData.Inventory.ContainerDataDict.GetValueOrDefault(id);
        }

        public ItemContainerData CreateContainerData(string id)
        {
            var data = new ItemContainerData
            {
                Id = id,
                SlotDataDict = new Dictionary<int, ItemSlotData>(),
            };
            _saveSystem.SaveData.Inventory.ContainerDataDict[id] = data;
            return data;
        }
        
    }
}