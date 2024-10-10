using Game.Framework;
using Game.Inventory.Models;

namespace Game.Farm
{
    public class Plot : Unit
    {
        
        public Crop Crop { get; private set; }
        
        public bool CanPlant()
        {
            return Crop == null;
        }

        public void Plant(Item plantItem)
        {
            
        }
        
    }
}