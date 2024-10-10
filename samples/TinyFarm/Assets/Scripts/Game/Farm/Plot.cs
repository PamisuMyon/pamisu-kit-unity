using Game.Configs;
using Game.Framework;
using Game.Inventory.Models;
using UnityEngine;

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
            if (plantItem.Config is not SeedConfig seedConfig)
            {
                Debug.LogError($"Plot plant item SeedConfig expected, got {plantItem.Config.GetType()}", Go);
                return;
            }
            
            plantItem.ChangeAmount(-1);
        }
        
    }
}