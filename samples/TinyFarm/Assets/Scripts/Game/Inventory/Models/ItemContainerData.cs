using System.Collections.Generic;
using Game.Framework;

namespace Game.Inventory.Models
{
    public class ItemContainerData : ISerializee
    {
        public string Id;
        public Dictionary<string, int> ItemSlotDict;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            ItemSlotDict ??= new Dictionary<string, int>();
        }
        
    }
}