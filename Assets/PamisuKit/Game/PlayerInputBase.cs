using System;
using Pamisu.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Pamisu.Game
{
    public class PlayerInputBase : MonoBehaviour, BasicInputAsset.IPlayerActions
    {
        
        [Header("Mouse Cursor Settings")]
        public bool CursorLocked;
        public bool InvertMouseY = true;

        [Header("Input Values")]
        public Vector2 Move;
        public Vector2 Look;
        public bool Sprint;
        public bool Jump;
        public bool JumpHeld;
        public bool Fire1;
        public bool Fire2;
        public bool Fire3;
        public bool Interact;

        public InputDevice CurrentDevice { get; protected set; }
        public Vector2 MousePosition => Mouse.current.position.ReadValue();
        
        protected BasicInputAsset input;
        private IDisposable _anyButtonEventListener;

        protected virtual void OnEnable()
        {
            if (input == null)
            {
                input = new BasicInputAsset();
                input.Player.SetCallbacks(this);
            }
            input.Player.Enable();
            _anyButtonEventListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
        }

        protected virtual void OnDisable()
        {
            input.Player.Disable();
            _anyButtonEventListener.Dispose();
        }

        public void Invalidate()
        {
            Move = Vector2.zero;
            Sprint = false;
            Jump = false;
            JumpHeld = false;
            Fire1 = false;
            Fire2 = false;
            Fire3 = false;
        }

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(CursorLocked);
        }
        
        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void OnAnyButtonPressed(InputControl button)
        {
            CurrentDevice = button.device;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.WasPressedThisFrame())
                Jump = true;
            JumpHeld = context.action.IsPressed();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Sprint = context.performed;
        }

        public void OnFire1(InputAction.CallbackContext context)
        {
            Fire1 = context.performed;
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            Fire2 = context.performed;
        }

        public void OnFire3(InputAction.CallbackContext context)
        {
            Fire3 = context.performed;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Interact = context.performed;
        }

    }
}