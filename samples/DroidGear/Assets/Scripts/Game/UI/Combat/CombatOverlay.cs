using Cysharp.Threading.Tasks;
using Game.Events;
using UnityEngine;
using Game.UI.Common;
using UnityEngine.AddressableAssets;
using System;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;

namespace Game.UI.Combat
{
    public class CombatOverlay : MonoEntity
    {
        [SerializeField]
        private AssetReferenceGameObject _floatingTextRef;

        private Camera _cam;
        private MonoPool<FloatingText> _damageTextPool;

        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        private async UniTaskVoid Init() 
        {
            _cam = Camera.main;
            _damageTextPool = await MonoPool<FloatingText>.Create(_floatingTextRef, transform, 64);
            On<ReqShowDamageText>(OnReqShowDamageText);
        }

        private void OnReqShowDamageText(ReqShowDamageText e)
        {
            ShowDamageText(e).Forget();
        }

        private async UniTaskVoid ShowDamageText(ReqShowDamageText e)
        {
            var floatingText = _damageTextPool.Spawn();
            if (floatingText == null)
                return;

            await floatingText.Popup(_cam, e.WorldPos, Math.Abs(e.Damage.Value).ToString("0"));
            _damageTextPool.Release(floatingText);
        }

    }
}