using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace Game.UI.Common
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]
        private float _duration = .5f;

        private RectTransform _body;
        private TMP_Text _text;

        private Camera _cam;
        private Vector3 _popupPos;
        private RectTransform _parentRect;
        private RectTransform _rect;

        private void Awake()
        {
            _parentRect = transform.parent as RectTransform;
            _rect = transform as RectTransform;
            _body = transform.Find("Body") as RectTransform;
            _text = _body.GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            var screenPos = _cam.WorldToScreenPoint(_popupPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, screenPos, null, out var localPos);
            _rect.localPosition = localPos;
        }

        public async UniTask Popup(Camera cam, Vector3 worldPos, string content)
        {
            gameObject.SetActive(true);
            _cam = cam;
            _popupPos = worldPos;
            _text.text = content;
            _body.anchoredPosition = Vector2.zero;

            await LMotion.Create(Vector2.zero, new Vector2(0, 50f), _duration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_body)
                .ToUniTask(destroyCancellationToken);

            gameObject.SetActive(false);
        }

    }
}