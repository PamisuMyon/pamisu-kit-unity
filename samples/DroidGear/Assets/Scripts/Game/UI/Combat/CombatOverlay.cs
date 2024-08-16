using Cysharp.Threading.Tasks;
using Game.Events;
using PamisuKit.Common;
using UnityEngine;
using Game.UI.Common;
using UnityEngine.AddressableAssets;
using System;
using PamisuKit.Common.Pool;

namespace Game.UI.Combat
{
    public class CombatOverlay : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceGameObject _floatingTextRef;

        private Camera _cam;
        private MonoPool<FloatingText> _damageTextPool;

        private void Awake()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init() 
        {
            _cam = Camera.main;
            _damageTextPool = await MonoPool<FloatingText>.Create(_floatingTextRef, transform, 64);
            EventBus.On<RequestShowDamageText>(OnRequestShowDamageText);
        }

        private void OnDestroy()
        {
            EventBus.Off<RequestShowDamageText>(OnRequestShowDamageText);
        }

        private void OnRequestShowDamageText(RequestShowDamageText e)
        {
            ShowDamageText(e).Forget();
        }

        private async UniTaskVoid ShowDamageText(RequestShowDamageText e)
        {
            FloatingText floatingText = _damageTextPool.Spawn();
            if (floatingText == null)
                return;

            await floatingText.Popup(_cam, e.WorldPos, Math.Abs(e.Damage.Value).ToString());
            _damageTextPool.Release(floatingText);
        }

    }
}