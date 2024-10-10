using Game.Inventory.Models;

namespace Game.Events
{
    public struct ReqPlayerControlStateReset { }
    
    public struct ReqPlayerControlEnterPlantState
    {
        public Item PlantItem;
    }
    
}