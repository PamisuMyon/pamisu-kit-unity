using System.Collections.Generic;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Inventory
{
    public class ItemCollection : MonoEntity
    {
        [SerializeField]
        private Item[] _initItems;
        
        public List<Item> Items { get; private set; }
        
        
    }
}