using Cysharp.Threading.Tasks;
using Game.Inventory.Models;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemSlot : MonoEntity
    {
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
                }
                _item = value;
                if (_item != null)
                {
                    _item.Changed += OnItemChanged;
                }
            }
        }

        private void OnItemChanged(Item item)
        {
            AmountLabel.text = item.Amount.ToString();
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

    }
}