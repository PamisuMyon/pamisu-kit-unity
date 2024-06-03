using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Common.Assets;
using UnityEngine;
using Game.UI.Common;
using UnityEngine.AddressableAssets;
using System;

namespace Game.UI.Combat
{
    public class CombatOverlay : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceGameObject _floatingTextRef;

        private RectTransform _rectTrans;
        private Camera _cam;

        private List<FloatingText> _floatingTextPool = new();
        private GameObject _floatingTextPrefab;

        private void Awake()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init() 
        {
            _rectTrans = transform as RectTransform;
            _cam = Camera.main;
            _floatingTextPrefab = await AssetManager.LoadAsset<GameObject>(_floatingTextRef);
            EventBus.On<RequestShowDamageText>(OnRequestShowDamageText);
        }

        private void OnDestroy()
        {
            EventBus.Off<RequestShowDamageText>(OnRequestShowDamageText);
        }

        private void OnRequestShowDamageText(RequestShowDamageText e)
        {
            FloatingText floatingText = null;
            for (int i = 0; i < _floatingTextPool.Count; i++)
            {
                if (!_floatingTextPool[i].gameObject.activeInHierarchy)
                {
                    floatingText = _floatingTextPool[i];
                    break;
                }
            }
            
            if (floatingText == null)
            {
                var go = Instantiate(_floatingTextPrefab, transform);
                floatingText = go.GetComponent<FloatingText>();
                _floatingTextPool.Add(floatingText);
            }

            var screenPos = _cam.WorldToScreenPoint(e.WorldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, screenPos, null, out var localPoint);
            floatingText.Popup(localPoint, Math.Abs(e.Damage.Value).ToString()).Forget();
        }

    }
}