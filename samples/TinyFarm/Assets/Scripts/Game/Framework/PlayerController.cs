using Game.Events;
using Game.Farm;
using Game.Inventory.Models;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using InputSystem = Game.Inputs.InputSystem;

namespace Game.Framework
{
    public class PlayerController : MonoEntity, IUpdatable
    {
        [SerializeField]
        private LayerMask _unitLayerMask;

        [SerializeField]
        private Camera _mainCam;

        [SerializeField]
        private Indicator _hoverIndicator;

        [SerializeField]
        private Indicator _selectionIndicator;

        private InputSystem _inputSystem;
        private Item _plantItem;
        private bool _isPointerOverUI;
        
        public PlayerControlState State { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            if (_mainCam == null)
                _mainCam = Camera.main;

            _hoverIndicator.Setup(Region);
            _selectionIndicator.Setup(Region);

            _inputSystem = GetSystem<InputSystem>();
            _inputSystem.Actions.Game.CursorPosition.performed += OnCursorPositionPerformed;
            _inputSystem.Actions.Game.CursorConfirm.performed += OnCursorConfirmPerformed;
            _inputSystem.Actions.Game.ToggleInventory.performed += OnToggleInventoryPerformed;

            On<ReqPlayerControlStateReset>(OnReqPlayerControlStateReset);
            On<ReqChangePlayerControlState>(ReqPlayerControlEnterPlantState);

            ChangeState(PlayerControlState.Normal);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            if (_inputSystem != null)
            {
                _inputSystem.Actions.Game.CursorPosition.performed -= OnCursorPositionPerformed;
                _inputSystem.Actions.Game.CursorConfirm.performed -= OnCursorConfirmPerformed;
                _inputSystem.Actions.Game.ToggleInventory.performed -= OnToggleInventoryPerformed;
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }

        private void OnCursorPositionPerformed(InputAction.CallbackContext c)
        {
            if (GetUnitUnderCursor(out var unit) 
                && _selectionIndicator.AttachedUnit != unit)
            {
                _hoverIndicator.Attach(unit);
            }
            else
            {
                _hoverIndicator.Detach();
            }
        }

        private void OnCursorConfirmPerformed(InputAction.CallbackContext c)
        {
            if (GetUnitUnderCursor(out var unit))
            {
                _selectionIndicator.Attach(unit);
                _hoverIndicator.Detach();
                OnUnitClicked(unit);
            }
            else
            {
                var detachedUnit = _selectionIndicator.Detach();
                if (detachedUnit != null)
                    detachedUnit.OnDeselected();
            }
        }
        
        private void OnToggleInventoryPerformed(InputAction.CallbackContext c)
        {
            Emit(new ReqToggleInventoryView());
        }
        
        private void OnReqPlayerControlStateReset(ReqPlayerControlStateReset e)
        {
            ChangeState(PlayerControlState.Normal);
            _plantItem = null;
        }

        private void ReqPlayerControlEnterPlantState(ReqChangePlayerControlState e)
        {
            ChangeState(PlayerControlState.Plant);
            _plantItem = e.Item;
        }

        private void ChangeState(PlayerControlState newState)
        {
            var oldState = State;
            State = newState;
            Emit(new PlayerControlStateChanged { OldState = oldState, NewState = newState });
        }

        private bool GetUnitUnderCursor(out Unit unit)
        {
            if (_isPointerOverUI)
            {
                unit = null;
                return false;
            }
            
            var cursorPos = _inputSystem.Actions.Game.CursorPosition.ReadValue<Vector2>();
            var ray = _mainCam.ScreenPointToRay(cursorPos);
            var hit = Physics2D.GetRayIntersection(ray, 100f, _unitLayerMask);
            Debug.DrawRay(ray.origin, ray.direction * 100f, hit ? Color.green : Color.red);
            if (hit && hit.collider.TryGetComponent(out unit))
                return true;
            unit = null;
            return false;
        }

        private void OnUnitClicked(Unit unit)
        {
            if (State == PlayerControlState.Plant)
            {
                if (unit is Plot plot && plot.CanPlant())
                {
                    plot.Plant(_plantItem);
                }
            }
            else if (State == PlayerControlState.Shovel)
            {
                if (unit is Plot plot)
                {
                    plot.RemoveCrop();
                }
            }
            else if (State == PlayerControlState.Normal)
            {
                unit.OnSelected();
            }
        }
        
    }

    public enum PlayerControlState
    {
        Disabled,
        Normal,
        Plant,
        Shovel,
    }
    
}