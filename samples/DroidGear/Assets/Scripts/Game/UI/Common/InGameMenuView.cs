using Game.Input;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.UI.Common
{
    public class InGameMenuView : MonoEntity
    {

        [SerializeField]
        private RectTransform _panel;
        
        [SerializeField]
        private Button _resumeButton;
        
        [SerializeField]
        private Button _titleButton;
        
        [SerializeField]
        private Button _quitButton;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _resumeButton.SetOnClickListener(() =>
            {
                Hide();
            });
            _titleButton.SetOnClickListener(() =>
            {
                App.Instance.LoadTitle();
            });
            _quitButton.SetOnClickListener(() =>
            {
                App.Instance.Quit();
            });

            GetSystem<InputWrapper>().Actions.Combat.Menu.performed += OnMenuPerformed;

            Hide();
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            var inputWrapper = GetSystem<InputWrapper>();
            if (inputWrapper != null)
            {
                inputWrapper.Actions.Combat.Menu.performed -= OnMenuPerformed;
            }
        }

        private void OnMenuPerformed(InputAction.CallbackContext c)
        {
            if (_panel.gameObject.activeInHierarchy)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            GetDirector<GameDirector>().PauseCombat();
            _panel.gameObject.SetActive(true);
        }

        public void Hide()
        {
            GetDirector<GameDirector>().ResumeCombat();
            _panel.gameObject.SetActive(false);
        }
    }
}