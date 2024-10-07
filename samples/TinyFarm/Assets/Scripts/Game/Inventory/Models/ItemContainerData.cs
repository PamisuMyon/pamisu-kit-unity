using System.Collections.Generic;
using Game.Framework;

namespace Game.Inventory.Models
{
    public class ItemContainerData : ISerializee
    {
        public string Id;
        public Dictionary<int, ItemSlotData> SlotDataDict;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            SlotDataDict ??= new Dictionary<int, ItemSlotData>();
        }
        
    }
}