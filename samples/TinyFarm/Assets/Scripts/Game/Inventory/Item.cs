using System;
using Game.Configs;
using UnityEngine;

namespace Game.Inventory
{
    [Serializable]
    public class Item
    {
        [SerializeField]
        private ItemConfig _config;

        [SerializeField]
        private float _amount;

        public ItemConfig Config => _config;
        public float Amount => _amount;

        public event Action<Item> Changed;
        
    }
}