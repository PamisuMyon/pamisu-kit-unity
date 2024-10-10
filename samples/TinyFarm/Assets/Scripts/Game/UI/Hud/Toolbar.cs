using LitMotion;
using LitMotion.Extensions;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class Toolbar : MonoEntity
    {
        [SerializeField]
        private float _showPosY;
        
        [SerializeField]
        private float _hidePosY;
        
        [SerializeField]
        private float _toggleAnimDuration = 0.3f;
        
        [Space]
        [SerializeField]
        private Button _shovelButton;

        [SerializeField]
        private Button _plantButton;

        [SerializeField]
        private PlantBar _plantBar;

        private RectTransform _rectTrans;
        private MotionHandle _toggleHandle;
        
        public bool IsShowing { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            _rectTrans = Trans as RectTransform;

            _plantBar.Setup(Region);
            _plantBar.Toggling += b => Toggle(!b);
            
            _plantButton.SetOnClickListener(() =>
            {
                _plantBar.Toggle(true);
            });
            
            Toggle(true, false, true);
        }

        public void Toggle(bool isShow, bool isAnim = true, bool force = false)
        {
            if (IsShowing == isShow && !force)
                return;
            IsShowing = isShow;

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