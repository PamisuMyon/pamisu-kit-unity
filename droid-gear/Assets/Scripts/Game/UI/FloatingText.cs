using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]
        private float _duration = .5f;

        [SerializeField]
        private TMP_Text _text;

        private RectTransform _rectTrans;

        private void Awake()
        {
            _rectTrans = transform as RectTransform;
            _text = GetComponentInChildren<TMP_Text>();
        }

        public async UniTaskVoid Popup(Vector2 localPos, string content)
        {
            gameObject.SetActive(true);
            _rectTrans.localPosition = localPos;
            _text.text = content;
            await UniTask.Delay(TimeSpan.FromSeconds(_duration), false, PlayerLoopTiming.Update, destroyCancellationToken);
            gameObject.SetActive(false);
        }

    }
}