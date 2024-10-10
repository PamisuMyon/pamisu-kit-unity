using System;
using Game.Configs;
using Game.Framework;
using OdinSerializer;
using UnityEngine;

namespace Game.Inventory.Models
{
    [Serializable]
    public class Item : ISerializee
    {
        [OdinSerialize]
        public string Id { get; private set; }
        [OdinSerialize]
        private string _configId;
        public ItemConfig Config { get; private set; }
        [OdinSerialize]
        public int Amount { get; private set; }
        public ItemCollection Collection { get; internal set; }
        
        public event Action<Item> Changed;
        public event Action<Item> Removing;

        public static Item Create(ItemConfig config, int amount = 1)
        {
            var item = new Item
            {
                Id = Guid.NewGuid().ToString(),
                _configId = config.Id,
                Config = config,
                Amount = amount,
            };
            return item;
        }

        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Config = App.Instance.GetSystem<ConfigSystem>().GetItemConfig(_configId);
            if (Config == null)
                Debug.LogError($"ItemConfig of Id {_configId} not found");
        }

        public void ChangeAmount(int delta)
        {
            Amount = Mathf.Max(0, Amount + delta);
            if (Amount == 0)
            {
                Collection.RemoveItem(this);
                return;
            }
            Changed?.Invoke(this);
        }

        public void NotifyRemoving()
        {
            Changed = null;
            Removing?.Invoke(this);
            Removing = null;
        }
        
    }
}