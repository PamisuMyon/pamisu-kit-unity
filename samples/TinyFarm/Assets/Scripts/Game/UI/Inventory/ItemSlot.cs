using Game.Inventory;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemSlot : MonoEntity
    {
        [SerializeField]
        protected Image IconImage;

        [SerializeField]
        protected TMP_Text AmountText;

        public ItemContainer Container { get; internal set; }
        public int Index { get; internal set; }

        public void SetItem(Item item)
        {
            
        }
        
    }
}