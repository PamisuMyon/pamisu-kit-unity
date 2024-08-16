using System;
using PamisuKit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Game.Input
{
    public class InputWrapper : MonoSystem
    {

        private GameInputActions _actions;
        private IDisposable _anyButtonEventListener;

        public GameInputActions Actions => _actions;
        public InputDevice CurrentDevice { get; set; }

        public void Init()
        {
            _actions = new GameInputActions();
            _actions.Combat.Enable();

            _anyButtonEventListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            _anyButtonEventListener?.Dispose();
        }

        private void OnAnyButtonPressed(InputControl control)
        {
            CurrentDevice = control.device;
        }

    }
}