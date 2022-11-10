using Pamisu.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pamisu.Game
{
    public class PlayerInputBase : MonoBehaviour, BasicInputAsset.IPlayerActions
    {
        
        [Header("Mouse Cursor Settings")]
        public bool CursorLocked = true;

        public bool CursorInputForLook = true;
        public float LookSensitivity = 1f;
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

        protected BasicInputAsset _input;


        private void OnEnable()
        {
            if (_input == null)
            {
                _input = new BasicInputAsset();
                _input.Player.SetCallbacks(this);
            }
            _input.Player.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Disable();
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

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(CursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
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