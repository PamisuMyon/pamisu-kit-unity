using Game.Common;
using Game.Configs;
using Game.Input;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using PamisuKit.Common.FSM;
using Game.Characters.Player.States;
using Game.Framework;
using PamisuKit.Common;
using Game.Events;
using Game.Characters.Drone;
using Game.Props;
using PamisuKit.Common.Util;

namespace Game.Characters.Player
{
    public class PlayerController : Framework.CharacterController, IUpdatable, IFixedUpdatable
    {

        [SerializeField]
        private float _turnSpeed = 720f;

        [SerializeField]
        private TriggerArea _pickupArea;

        public DroneController Drone;

        internal float MoveSpeed;
        internal Vector3 Movement;
        // internal UnityEngine.CharacterController Cc { get; private set; }
        internal Rigidbody Rb { get; private set; }
        public StateMachine Fsm { get; private set; }
        // public PlayerStates.Blackboard Bb { get; private set; }
        
        public override void Init(CharacterConfig config)
        {
            base.Init(config);
            // Cc = GetComponent<UnityEngine.CharacterController>();
            Rb = GetComponent<Rigidbody>();
            MoveSpeed = Chara.AttrComp[AttributeType.MoveSpeed].Value;

            Drone.Setup(Region);
            Drone.Init((config as HeroConfig).DroneConfig, Chara);

            _pickupArea.TriggerEnter += OnPickupAreaEnter;
            var input = GetSystem<InputWrapper>();
            input.Actions.Combat.Move.started += OnMove;
            input.Actions.Combat.Move.performed += OnMove;
            input.Actions.Combat.Move.canceled += OnMove;

            // Bb = new PlayerStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new PlayerStates.Normal(this));
            Fsm.AddState(new PlayerStates.Death(this));
            Fsm.ChangeState<PlayerStates.Normal>();
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            Fsm?.OnDestroy();
            _pickupArea.TriggerEnter -= OnPickupAreaEnter;
            var input = GetSystem<InputWrapper>();
            if (input != null)
            {
                input.Actions.Combat.Move.started -= OnMove;
                input.Actions.Combat.Move.performed -= OnMove;
                input.Actions.Combat.Move.canceled -= OnMove;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm?.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            Fsm?.OnFixedUpdate(deltaTime);
        }

        protected override void OnDie(Character character)
        {
            base.OnDie(character);
            if (Fsm.CurrentState is not PlayerStates.Death) 
            {
                Fsm.ChangeState<PlayerStates.Death>();
                Drone.Die();
                EventBus.Emit(new PlayerDie());
            }
        }
        
        #region Input Actions

        private void OnMove(InputAction.CallbackContext c)
        {
            var input = c.ReadValue<Vector2>();
            Movement = new Vector3(input.x, 0f, input.y);
        }

        #endregion

        private void OnPickupAreaEnter(Collider col)
        {
            if (col.CompareTag("Pickup") 
                && col.TryGetComponentInDirectParent<Pickup>(out var pickup)
                && pickup.CanPickup)
            {
                pickup.Pick(Chara);
            }
        }

        internal void HandleMovement(float deltaTime) 
        {
            if (Movement != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(Movement);
                var rotation = Model.transform.rotation;
                Model.transform.rotation = Quaternion.RotateTowards(rotation, targetRotation, _turnSpeed * deltaTime);
            }

            // Cc.Move(MoveSpeed * deltaTime * Movement);
            Rb.velocity = MoveSpeed * Region.Ticker.TimeScale * Movement;
        }

    }
}