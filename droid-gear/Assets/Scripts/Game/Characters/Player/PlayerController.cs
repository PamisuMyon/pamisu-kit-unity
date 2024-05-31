using Game.Common;
using Game.Configs;
using Game.Input;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using PamisuKit.Common.FSM;
using Game.Characters.Player.States;
using Game.Framework;
using PamisuKit.Common.Util;

namespace Game.Characters.Player
{
    public class PlayerController : Framework.CharacterController, IUpdatable, IFixedUpdatable
    {

        [SerializeField]
        private float _turnSpeed = 720f;

        [SerializeField]
        private TriggerArea _senseArea;

        [SerializeField]
        private TriggerArea _pickupArea;

        public PlayerDrone Drone;

        internal float MoveSpeed;
        internal Vector3 Movement;
        public StateMachine Fsm { get; private set; }
        public PlayerStates.Blackboard Bb { get; private set; }
        
        public override void Init(CharacterConfig config)
        {
            base.Init(config);
            MoveSpeed = Chara.AttrComp[AttributeType.MoveSpeed].Value;

            Drone.SetupEntity(Region, false);
            Drone.Init(Chara);

            _pickupArea.TriggerEnter += OnPickupAreaEnter;
            _senseArea.TriggerEnter += OnSenseAreaEnter;
            _senseArea.TriggerExit += OnSenseAreaExit;
            InputWrapper.Actions.Combat.Move.started += OnMove;
            InputWrapper.Actions.Combat.Move.performed += OnMove;
            InputWrapper.Actions.Combat.Move.canceled += OnMove;

            Bb = new PlayerStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new PlayerStates.Normal(this));
            Fsm.AddState(new PlayerStates.Death(this));
            Fsm.ChangeState<PlayerStates.Normal>();
        }

        protected override void OnSelfDestroy()
        {
            Fsm?.OnDestroy();
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
            Fsm?.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            Fsm?.OnFixedUpdate(deltaTime);
        }
        
        #region Input Actions

        private void OnMove(InputAction.CallbackContext c)
        {
            var input = c.ReadValue<Vector2>();
            Movement = new Vector3(input.x, 0f, input.y);
        }

        #endregion

        private void OnPickupAreaEnter(Collider collider)
        {
        }

        private void OnSenseAreaEnter(Collider collider)
        {
            if (!collider.CompareTag("Enemy"))
                return;
            if (collider.gameObject.TryGetComponentInDirectParent(out Character character))
            {
                Bb.Targets.AddUnique(character);
            }
        }

        private void OnSenseAreaExit(Collider collider)
        {
            if (!collider.CompareTag("Enemy"))
                return;
            if (collider.gameObject.TryGetComponentInDirectParent(out Character character))
            {
                Bb.Targets.Remove(character);
                if (Bb.Target == character) 
                {
                    Bb.Target = null;
                    Drone.Target = null;
                }
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
            Chara.Rb.velocity = MoveSpeed * Movement * Region.Ticker.TimeScale;
        }

    }
}