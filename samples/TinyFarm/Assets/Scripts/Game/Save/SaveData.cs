using Game.Framework;
using Game.Inventory.Models;

namespace Game.Save
{
    public class SaveData : ISerializee
    {
        public InventoryData Inventory;
        
        public void PreSerialize()
        {
            Inventory.PreSerialize();
        }

        public void PostDeserialize()
        {
            Inventory ??= new InventoryData();
            
            Inventory.PostDeserialize();
        }
    }


}