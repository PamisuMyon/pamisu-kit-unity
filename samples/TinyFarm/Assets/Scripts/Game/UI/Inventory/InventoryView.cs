using System;
using Game.Events;
using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class InventoryView : ToggleableView
    {
        [Header("InventoryView")]
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private ItemContainer _itemsContainer;

        protected override void OnCreate()
        {
            base.OnCreate();
            _closeButton.SetOnClickListener(() =>
            {
                Hide();
            });
            _itemsContainer.Setup(Region);

            On<ReqToggleInventoryView>(OnReqToggleInventoryView);
            
            Hide(false, true);
        }

        private void OnReqToggleInventoryView(ReqToggleInventoryView e)
        {
            var b = e.NewState switch
            {
                ToggleState.Inverse => !IsShowing,
                ToggleState.On => true,
                ToggleState.Off => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            if (b)
                Show();
            else
                Hide();
        }
        
    }
}