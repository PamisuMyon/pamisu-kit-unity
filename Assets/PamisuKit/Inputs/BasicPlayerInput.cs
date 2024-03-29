﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Pamisu.Inputs
{
    public class BasicPlayerInput : MonoBehaviour
    {
        
        [Header("Mouse Cursor Settings")]
        public bool CursorLocked;

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
        public bool Menu;

        public InputDevice CurrentDevice { get; set; }
        public Vector2 MousePosition => Mouse.current.position.ReadValue();
        
        public BasicInputAsset Asset { get; protected set; }
        private BasicActionsImpl actionsImpl;
        
        protected IDisposable _anyButtonEventListener;

        protected virtual void OnEnable()
        {
            if (Asset == null)
            {
                Asset = new BasicInputAsset();
                actionsImpl = new BasicActionsImpl(this);
                Asset.Player.SetCallbacks(actionsImpl);
                Asset.Menu.SetCallbacks(actionsImpl);
            }
            Asset.Player.Enable();
            _anyButtonEventListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
        }

        protected virtual void OnDisable()
        {
            Asset.Player.Disable();
            _anyButtonEventListener.Dispose();
        }

        public virtual void Invalidate()
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
        
        protected void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void OnAnyButtonPressed(InputControl button)
        {
            CurrentDevice = button.device;
        }
        
    }

    class BasicActionsImpl : 
        BasicInputAsset.IPlayerActions,
        BasicInputAsset.IMenuActions
    {
        private readonly BasicPlayerInput p;

        public BasicActionsImpl(BasicPlayerInput input)
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
        
    }
}