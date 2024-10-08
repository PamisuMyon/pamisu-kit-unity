using Cysharp.Threading.Tasks;
using Game.Inputs;
using Game.Inventory.Models;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemSlot : MonoEntity,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField]
        protected bool Draggable = true;
        
        [Space]
        [SerializeField]
        protected Image FrameImage;

        [SerializeField]
        protected Sprite FrameEmptySprite;

        [SerializeField]
        protected Sprite FrameNormalSprite;

        [SerializeField]
        protected Image IconImage;
        
        [SerializeField]
        protected RectTransform AmountBadge;

        [SerializeField]
        protected TMP_Text AmountLabel;

        [SerializeField]
        protected GameObject DragDummyPrefab;

        private RectTransform _rectTrans;

        public ItemContainer Container { get; internal set; }
        public int Index { get; internal set; }

        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                if (_item != null)
                {
                    _item.Changed -= OnItemChanged;
                    _item.Removing -= OnItemRemoving;
                }
                _item = value;
                if (_item != null)
                {
                    _item.Changed += OnItemChanged;
                    _item.Removing += OnItemRemoving;
                }
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _rectTrans = Trans as RectTransform;
        }

        private void OnItemChanged(Item item)
        {
            AmountLabel.text = item.Amount.ToString();
        }
        
        private void OnItemRemoving(Item item)
        {
            _item = null;
            Refresh();
        }

        public void Refresh()
        {
            if (_item != null)
            {
                FrameImage.sprite = FrameNormalSprite;
                IconImage.color = Color.white;
                IconImage.LoadSprite(_item.Config.IconRef).Forget();
                AmountBadge.gameObject.SetActive(true);
                AmountLabel.text = _item.Amount.ToString();
            }
            else
            {
                FrameImage.sprite = FrameEmptySprite;
                IconImage.color = UnityUtil.TransparentWhite;
                AmountBadge.gameObject.SetActive(false);
            }
        }

        public void UseItem()
        {
            
        }

        #region Drag and drop

        public bool CanDrag()
        {
            return Draggable && Item != null;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanDrag())
                return;
            
            var gameUI = GetService<GameUI>();
            var dummy = gameUI.Pooler.Spawn<ItemDragDummy>(DragDummyPrefab);
            dummy.Setup(Region);
            dummy.SetData(gameUI.Pooler, this);
            
            // Offset
            var cursorPos = GetSystem<InputSystem>().Actions.Game.CursorPosition.ReadValue<Vector2>();
            var cursorWorldPos = gameUI.UICam.ScreenToWorldPoint(cursorPos);
            var offset = _rectTrans.position - cursorWorldPos;
            offset.z = 0f;

            var dragHelper = GetService<DragHelper>();
            dragHelper.BeginDrag(dummy, offset);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // TODO Drag to world
            
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var dragHelper = GetService<DragHelper>();
            if (!CanDrag()
                || dragHelper.DragDummy is not ItemDragDummy dummy
                || dummy.Slot == this)
            {
                dragHelper.EndDrag();
                return;
            }
            
        }

        #endregion

    }
}