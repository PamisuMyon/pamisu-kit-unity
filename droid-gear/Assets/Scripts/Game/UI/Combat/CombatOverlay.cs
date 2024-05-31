using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace Game.UI.Combat
{
    // TODO TEMP
    public class CombatOverlay : MonoBehaviour
    {
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
            _floatingTextPrefab = await AssetManager.LoadAsset<GameObject>("Assets/Res/UI/Combat/FloatingText.prefab");
            EventBus.On<RequestShowFloatingText>(OnRequestShowFloatingText);
        }

        private void OnDestroy()
        {
            EventBus.Off<RequestShowFloatingText>(OnRequestShowFloatingText);
        }

        private void OnRequestShowFloatingText(RequestShowFloatingText e)
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
            // Debug.Log($"Screen pos {screenPos}   local pos: {localPoint}");
            floatingText.Popup(localPoint, e.Content).Forget();
        }

    }
}