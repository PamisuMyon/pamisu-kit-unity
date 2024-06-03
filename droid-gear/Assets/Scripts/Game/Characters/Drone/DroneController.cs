using Game.Framework;
using PamisuKit.Common.FSM;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using Game.Characters.Drone.States;
using UnityEngine;
using Game.Common;
using Game.Configs;

namespace Game.Characters.Drone
{
    public class DroneController : Framework.CharacterController, IUpdatable
    {
        public float OrbitSpeed = 180f;
        public float OrbitRadius = .7f;
        public float TargetingFrequency = .5f;

        [SerializeField]
        private TriggerArea _senseArea;
        private SphereCollider _senseAreaCollider;

        internal float CurrentAngle;
        internal Ability AttackAbility;

        public StateMachine Fsm { get; private set; }
        public DroneStates.Blackboard Bb { get; private set; }
        public Character Owner { get; private set; }

        public void Init(CharacterConfig config, Character owner) 
        {
            base.Init(config);
            Owner = owner;
            var offset = transform.localPosition;
            CurrentAngle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;

            Chara.AbilityComp.TryGetAbility(config.AttackAbility.Id, out AttackAbility);

            _senseAreaCollider = _senseArea.GetComponent<SphereCollider>();
            _senseAreaCollider.radius = AttackAbility.Config.ActRange;
            _senseArea.TriggerEnter += OnSenseAreaEnter;
            _senseArea.TriggerExit += OnSenseAreaExit;

            Bb = new DroneStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new DroneStates.Idle(this));
            Fsm.AddState(new DroneStates.Attack(this));
            Fsm.AddState(new DroneStates.Death(this));
            Fsm.ChangeState<DroneStates.Idle>();
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            Fsm?.OnDestroy();
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm?.OnUpdate(deltaTime);
        }

        internal void UpdatePosition()
        {
            var x = OrbitRadius * Mathf.Cos(CurrentAngle * Mathf.Deg2Rad);
            var z = OrbitRadius * Mathf.Sin(CurrentAngle * Mathf.Deg2Rad);

            Trans.localPosition = new Vector3(x, Trans.localPosition.y, z);
        }

        internal Character SelectTarget()
        {
            if (Bb.Targets.Count == 0)
                return null;
            var minDis = float.MaxValue;
            Character minTarget = null;
            for (int i = Bb.Targets.Count - 1; i >= 0; i--)
            {
                if (Bb.Targets[i] == null || !Bb.Targets[i].IsAlive)
                {
                    Bb.Targets.RemoveAt(i);
                    continue;
                }
                var dis = Bb.Targets[i].Trans.position - Chara.Trans.position;
                if (dis.sqrMagnitude < minDis)
                {
                    minDis = dis.sqrMagnitude;
                    minTarget = Bb.Targets[i];
                }
            }
            return minTarget;
        }

        public void Die()
        {
            Fsm.ChangeState<DroneStates.Death>();
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
                }
            }
        }

    }
}
