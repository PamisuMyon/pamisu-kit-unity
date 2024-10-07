
using System.Collections.Generic;
using Game.Framework;

namespace Game.Inventory.Models
{
    public class InventoryData : ISerializee
    {
        public Dictionary<string, ItemCollectionData> CollectionDataDict;
        public Dictionary<string, ItemContainerData> ContainerDataDict;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            CollectionDataDict ??= new Dictionary<string, ItemCollectionData>();
            ContainerDataDict ??= new Dictionary<string, ItemContainerData>();

            foreach (var it in CollectionDataDict.Values)
                it.PostDeserialize();
            foreach (var it in ContainerDataDict.Values)
                it.PostDeserialize();
        }
    }
}