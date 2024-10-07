using System;
using Game.Configs;
using Game.Framework;

namespace Game.Inventory.Models
{
    [Serializable]
    public class Item : ISerializee
    {
        public string Id { get; private set; }
        public ItemConfig Config { get; private set; }
        public float Amount { get; private set; }

        public event Action<Item> Changed;
        public event Action<Item> Removing;

        public static Item Create(ItemConfig config, int amount = 1)
        {
            var item = new Item
            {
                Id = Guid.NewGuid().ToString(),
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
        }
        
    }
}