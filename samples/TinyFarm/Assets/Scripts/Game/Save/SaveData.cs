using Game.Framework;
using Game.Inventory.Models;
using Game.Worker.Models;

namespace Game.Save
{
    public class SaveData : ISerializee
    {
        public InventoryData Inventory;

        public WorkerSystemData Worker;
        
        public void PreSerialize()
        {
            Inventory.PreSerialize();
            Worker.PreSerialize();
        }

        public void PostDeserialize()
        {
            Inventory ??= new InventoryData();
            Worker ??= new WorkerSystemData();
            
            Inventory.PostDeserialize();
            Worker.PostDeserialize();
        }
    }

}