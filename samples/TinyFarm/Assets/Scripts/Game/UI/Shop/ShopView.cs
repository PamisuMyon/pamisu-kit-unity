using System;
using Game.Events;
using Game.UI.Inventory;
using UnityEngine;

namespace Game.UI.Shop
{
    public class ShopView : ToggleableView
    {
        [Header("ShopView")]
        [SerializeField]
        private InventoryView _inventoryView;
        
        protected override void OnCreate()
        {
            base.OnCreate();

            _inventoryView.Toggling += b =>
            {
                if (!b && IsShowing)
                    Hide();
            };

            On<ReqToggleShopView>(OnReqToggleShopView);
            
            Hide(false, true);
            Go.SetActive(false);
        }

        private void OnReqToggleShopView(ReqToggleShopView e)
        {
            var b = e.NewState switch
            {
                ToggleState.Inverse => !IsShowing,
                ToggleState.On => true,
                ToggleState.Off => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            if (b)
            {
                Go.SetActive(true);
                Show();
                _inventoryView.Show();
            }
            else
            {
                Hide();
                _inventoryView.Hide();
            }
        }

        protected override void OnHideAnimComplete()
        {
            Go.SetActive(false);
        }
        
    }
}