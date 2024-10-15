using Game.Farm.Models;
using Game.Framework;
using Game.Inventory.Models;
using Game.Worker.Models;

namespace Game.Save
{
    public class SaveData : ISerializee
    {
        public InventoryData Inventory;

        public WorkerSystemData Worker;

        public FarmData Farm;
        
        public void PreSerialize()
        {
            Inventory.PreSerialize();
            Worker.PreSerialize();
            Farm.PreSerialize();
        }

        public void PostDeserialize()
        {
            Inventory ??= new InventoryData();
            Worker ??= new WorkerSystemData();
            Farm ??= new FarmData();
            
            Inventory.PostDeserialize();
            Worker.PostDeserialize();
            Farm.PostDeserialize();
        }
    }

}