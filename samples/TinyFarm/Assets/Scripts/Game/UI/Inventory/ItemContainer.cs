using Game.Inventory;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI.Inventory
{
    public class ItemContainer : MonoEntity
    {
        [SerializeField]
        private ItemSlot _slots;

        [SerializeField]
        private ItemCollection _collection;
        
        
    }
}