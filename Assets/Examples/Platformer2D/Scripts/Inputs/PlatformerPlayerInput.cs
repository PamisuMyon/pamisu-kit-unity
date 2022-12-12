using Pamisu.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Pamisu.Platformer2D.Inputs
{
    public class PlatformerPlayerInput : BasicPlayerInput
    {
        public bool Dash;
        
        public new PlatformerInputAsset Asset { get; protected set; }
        private PlatformerActionsImpl actionsImpl;
        
        protected override void OnEnable()
        {
            if (Asset == null)
            {
                Asset = new PlatformerInputAsset();
                actionsImpl = new PlatformerActionsImpl(this);
                Asset.Player.SetCallbacks(actionsImpl);
                Asset.Menu.SetCallbacks(actionsImpl);
            }
            Asset.Player.Enable();
            _anyButtonEventListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
        }

        protected override void OnDisable()
        {
            Asset.Player.Disable();
            _anyButtonEventListener.Dispose();
        }

        public override void Invalidate()
        {
            base.Invalidate();
            Dash = false;
        }
        
    }

    class PlatformerActionsImpl :
        PlatformerInputAsset.IPlayerActions,
        PlatformerInputAsset.IMenuActions
    {
        private readonly PlatformerPlayerInput p;

        public PlatformerActionsImpl(PlatformerPlayerInput input)
        {
            p = input;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            p.Move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            p.Look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.WasPressedThisFrame())
                p.Jump = true;
            p.JumpHeld = context.action.IsPressed();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            p.Sprint = context.performed;
        }

        public void OnFire1(InputAction.CallbackContext context)
        {
            p.Fire1 = context.performed;
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            p.Fire2 = context.performed;
        }

        public void OnFire3(InputAction.CallbackContext context)
        {
            p.Fire3 = context.performed;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            p.Interact = context.performed;
        }

        public void OnMenu(InputAction.CallbackContext context)
        {
            p.Menu = context.performed;
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            p.Dash = context.performed;
        }
    }
}