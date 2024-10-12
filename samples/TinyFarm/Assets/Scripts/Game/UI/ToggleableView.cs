using System;
using LitMotion;
using LitMotion.Extensions;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI
{
    public class ToggleableView : MonoEntity
    {
        [Header("Toggleable")]
        [SerializeField]
        protected float ShowPosY;
        
        [SerializeField]
        protected float HidePosY;

        [SerializeField]
        protected float ToggleAnimDuration = 0.3f;

        [SerializeField]
        protected RectTransform ToggleRectTrans;
        
        protected MotionHandle ToggleHandle;
        
        public bool IsShowing { get; protected set; }
        public event Action<bool> Toggling;

        protected override void OnCreate()
        {
            base.OnCreate();
            if (ToggleRectTrans == null)
                ToggleRectTrans = Trans as RectTransform;
        }

        public virtual void Show(bool isAnim = true, bool force = false)
        {
            if (IsShowing && !force)
                return;
            IsShowing = true;
            Toggling?.Invoke(true);
            
            var pos = ToggleRectTrans.anchoredPosition;
            var targetPos = pos;
            targetPos.y = ShowPosY;
            if (ToggleHandle.IsActive())
                ToggleHandle.Cancel();
            if (isAnim)
            {
                ToggleHandle = LMotion.Create(pos, targetPos, ToggleAnimDuration)
                    .WithEase(Ease.OutCubic)
                    .BindToAnchoredPosition(ToggleRectTrans);
            }
            else
            {
                ToggleRectTrans.anchoredPosition = targetPos;
            }
        }

        public virtual void Hide(bool isAnim = true, bool force = false)
        {
            if (!IsShowing && !force)
                return;
            IsShowing = false;
            Toggling?.Invoke(false);

            var pos = ToggleRectTrans.anchoredPosition;
            var targetPos = pos;
            targetPos.y = HidePosY;
            if (ToggleHandle.IsActive())
                ToggleHandle.Cancel();
            if (isAnim)
            {
                ToggleHandle = LMotion.Create(pos, targetPos, ToggleAnimDuration)
                    .WithEase(Ease.OutCubic)
                    .WithOnComplete(OnHideAnimComplete)
                    .BindToAnchoredPosition(ToggleRectTrans);
            }
            else
            {
                ToggleRectTrans.anchoredPosition = targetPos;
            }
        }

        protected virtual void OnHideAnimComplete()
        {
        }
    }
}