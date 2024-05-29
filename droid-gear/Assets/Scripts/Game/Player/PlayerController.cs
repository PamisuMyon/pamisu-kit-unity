using Game.Common;
using Game.Configs;
using Game.Input;
using PamisuKit.Framework;
using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : Framework.CharacterController, IUpdatable, IFixedUpdatable
    {
        [SerializeField]
        private float _moveSpeed = 10f;

        [SerializeField]
        private float _turnSpeed = 720f;

        [SerializeField]
        private TriggerArea _pickupArea;

        private Vector3 _movement;
        
        public override void Init(CharacterConfig config)
        {
            base.Init(config);
            _pickupArea.TriggerEnter += OnPickupAreaEnter;
            InputWrapper.Actions.Combat.Move.started += OnMove;
            InputWrapper.Actions.Combat.Move.performed += OnMove;
            InputWrapper.Actions.Combat.Move.canceled += OnMove;
        }

        protected override void OnSelfDestroy()
        {
            _pickupArea.TriggerEnter -= OnPickupAreaEnter;
            if (InputWrapper.Instance != null)
            {
                InputWrapper.Actions.Combat.Move.started -= OnMove;
                InputWrapper.Actions.Combat.Move.performed -= OnMove;
                InputWrapper.Actions.Combat.Move.canceled -= OnMove;
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            Chara.Model.Anim.SetBool(AnimConst.IsRunning, _movement != Vector3.zero);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            HandleMovement(deltaTime);
        }
        
        #region Input Actions

        private void OnMove(InputAction.CallbackContext c)
        {
            var input = c.ReadValue<Vector2>();
            _movement = new Vector3(input.x, 0f, input.y);
        }

        #endregion

        private void OnPickupAreaEnter(Collider collider)
        {
        }

        internal void HandleMovement(float deltaTime) 
        {
            if (_movement != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(_movement);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * deltaTime);
            }
            Chara.Rb.velocity = _moveSpeed * _movement;
        }

        internal bool RotateTowards(Vector3 direction, float rotateSpeed = 0)
        {
            if (rotateSpeed == 0) rotateSpeed = _turnSpeed;
            var targetRotation = Quaternion.LookRotation(direction);
            var rotation = transform.rotation;
            rotation = Quaternion.RotateTowards(rotation, targetRotation, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
            return rotation.Approximately(targetRotation, 0.005f);
        }

    }
}