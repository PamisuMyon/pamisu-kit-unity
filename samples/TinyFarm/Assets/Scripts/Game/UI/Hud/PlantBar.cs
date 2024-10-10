using System;
using Game.UI.Inventory;
using LitMotion;
using LitMotion.Extensions;
using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class PlantBar : ItemContainer
    {
        [Header("PlantBar")]
        [SerializeField]
        private float _showPosY;
        
        [SerializeField]
        private float _hidePosY;

        [SerializeField]
        private float _toggleAnimDuration = 0.3f;
        
        [Space]
        [SerializeField]
        private Button _closeButton;

        private RectTransform _rectTrans;
        private MotionHandle _toggleHandle;
        
        public bool IsShowing { get; private set; }
        
        public event Action<bool> Toggling;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            _rectTrans = Trans as RectTransform;
            _closeButton.SetOnClickListener(() =>
            {
                Toggle(false);
            });
            Toggle(false, false, true);
        }

        public void Toggle(bool isShow, bool isAnim = true, bool force = false)
        {
            if (IsShowing == isShow && !force)
                return;
            IsShowing = isShow;

            Toggling?.Invoke(isShow);
            var pos = _rectTrans.anchoredPosition;
            var targetPos = pos;
            targetPos.y = isShow? _showPosY : _hidePosY;
            if (isAnim)
            {
                if (_toggleHandle.IsActive())
                    _toggleHandle.Cancel();
                _toggleHandle = LMotion.Create(pos, targetPos, _toggleAnimDuration)
                    .WithEase(Ease.OutCubic)
                    .BindToAnchoredPosition(_rectTrans);
            }
            else
            {
                _rectTrans.anchoredPosition = targetPos;
            }
        }
        
    }
}