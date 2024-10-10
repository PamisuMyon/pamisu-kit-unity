using Game.Events;
using Game.Inventory.Models;
using Game.UI.Inventory;
using PamisuKit.Common.Pool;
using TMPro;
using UnityEngine;

namespace Game.UI.Hud
{
    public class PlantDragDummy : ItemDragDummy
    {
        [Header("PlantDragDummy")]
        [SerializeField]
        protected TMP_Text AmountLabel;
        
        public override void SetData(MonoPooler pooler, ItemSlot slot)
        {
            base.SetData(pooler, slot);
            Slot.Item.Changed += OnItemChanged;
            Slot.Item.Removing += OnItemRemoving;
            OnItemChanged(Slot.Item);
        }

        private void OnItemChanged(Item item)
        {
            AmountLabel.text = item.Amount.ToString();
        }

        private void OnItemRemoving(Item item)
        {
            GetService<ClickDragHelper>().EndDrag();
        }

        public override void OnBeginDrag()
        {
            base.OnBeginDrag();
            Emit(new ReqPlayerControlEnterPlantState
            {
                PlantItem = Slot.Item
            });
        }

        public override void OnEndDrag()
        {
            Emit(new ReqPlayerControlStateReset());
            if (Slot.Item != null)
            {
                Slot.Item.Changed -= OnItemChanged;
                Slot.Item.Removing -= OnItemRemoving;
            }
            base.OnEndDrag();
        }
        
    }
}