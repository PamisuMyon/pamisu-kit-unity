using System;
using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Upgrades;
using LitMotion;
using LitMotion.Extensions;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class UpgradeItemView : MonoEntity
    {

        [SerializeField]
        private float _animShowStartPosY;
        
        [SerializeField]
        private float _animHideEndPosY;
        
        [SerializeField]
        private RectTransform _contentPanel;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _description;

        [SerializeField]
        private Button _button;

        private UpgradeItem _item;

        public Action<UpgradeItem> ItemClicked;

        protected override void OnCreate()
        {
            base.OnCreate();
            _button.SetOnClickListener(() =>
            {
                ItemClicked?.Invoke(_item);
            });
        }

        public void SetData(UpgradeItem item)
        {
            _item = item;
            if (item.IsUnlockCharacter)
            {
                _icon.LoadSprite(item.Chara.IconRef).Forget();
                _description.text = item.Chara.DisplayName;
            }
            else
            {
                _icon.LoadSprite(item.Upgrade.IconRef).Forget();
                _description.text = item.Upgrade.Description;
            }
        }

        public async UniTaskVoid Show(float animDuration)
        {
            _button.enabled = false;
            var pos = new Vector2(0, _animShowStartPosY);
            await LMotion.Create(pos, Vector2.zero, animDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_contentPanel)
                .ToUniTask();
            _button.enabled = true;
        }

        public UniTask Hide(float animDuration)
        {
            _button.enabled = false;
            var pos = new Vector2(0, _animHideEndPosY);
            return LMotion.Create(Vector2.zero, pos, animDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_contentPanel)
                .ToUniTask();
        }
        
    }
}