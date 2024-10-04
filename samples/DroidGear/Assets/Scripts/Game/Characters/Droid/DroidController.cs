using Game.Configs;
using Game.Framework;
using PamisuKit.Common.FSM;
using PamisuKit.Common.Util;
using UnityEngine.AI;
using Game.Characters.Droid.States;
using PamisuKit.Framework;
using UnityEngine;
using Game.Common;
using Game.Characters.Player;
using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Combat;
using System.Collections.Generic;

namespace Game.Characters
{
    public class DroidController : Framework.CharacterController, IUpdatable
    {
        [SerializeField]
        private TriggerArea _senseArea;
        // private SphereCollider _senseAreaCollider;

        public float TrackFrequency = .2f;
        public float TargetingFrequency = 2f;
        public float TurnSpeed = 720f;
        public Vector2 WanderRange = new(2f, 6f);

        public DroidConfig DroidConfig { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public StateMachine Fsm { get; private set; }
        public DroidStates.Blackboard Bb { get; private set; }

        public override void Init(CharacterConfig config)
        {
            base.Init(config);
            DroidConfig = config as DroidConfig;
            Chara.AttrComp.HealthChanged += OnHealthChanged;

            Agent = GetComponent<NavMeshAgent>();

            Bb = new DroidStates.Blackboard();
            Debug.Assert(Config.AttackAbility != null, "AttackAbility is null", Go);
            if (Chara.AbilityComp.TryGetAbility(Config.AttackAbility.Id, out var attackAbility))
                Bb.AttackAbility = attackAbility;
            UniTask.Action(async () => 
            {
                await UniTask.Yield();
                // Bb.Player = FindFirstObjectByType<PlayerController>().Chara;
                Bb.Player = GetSystem<CombatSystem>().Bb.Player.Chara;
            })();

            // _senseAreaCollider = _senseArea.GetComponent<SphereCollider>();
            _senseArea.TriggerEnter += OnSenseAreaEnter;
            _senseArea.TriggerExit += OnSenseAreaExit;

            Fsm = new StateMachine();
            Fsm.AddState(new DroidStates.Idle(this));
            Fsm.AddState(new DroidStates.Wander(this));
            Fsm.AddState(new DroidStates.Track(this));
            Fsm.AddState(new DroidStates.Attack(this));
            Fsm.AddState(new DroidStates.Death(this));
            Fsm.ChangeState<DroidStates.Idle>();

            On<PlayerDie>(OnPlayerDie);
            On<EnemySpotted>(OnEnemySpotted);
        }

        protected override void OnSelfDestroy()
        {
            _senseArea.TriggerEnter -= OnSenseAreaEnter;
            _senseArea.TriggerExit -= OnSenseAreaExit;
            Fsm?.OnDestroy();
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm.OnUpdate(deltaTime);
        }

        protected virtual void OnHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            if (newHealth <= 0)
                Fsm.ChangeState<DroidStates.Death>();
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
                // dont clear current target
                // if (Bb.Target == character) 
                // {
                //     Bb.Target = null;
                // }
            }
        }

        private void OnPlayerDie(PlayerDie e)
        {
            if (Fsm.CurrentState is not DroidStates.Death)
                Fsm.ChangeState<DroidStates.Death>();
        }

        private void OnEnemySpotted(EnemySpotted e)
        {
            if (e.Instigator == Chara)
                return;
            var currentState = Fsm.CurrentState;
            if (currentState is DroidStates.Death
                || (currentState is DroidStates.Track
                    && !Bb.ShouldReturnToPlayer)
                || currentState is DroidStates.Attack)
                return;
            Bb.Target = e.Target;
            Bb.ShouldReturnToPlayer = false;
            Fsm.ChangeState<DroidStates.Track>();
            return;
        }

        internal Character SelectTarget()
        {
            List<Character> targets = null;
            if (Bb.Targets.Count != 0)
                targets = Bb.Targets;
            else if (Bb.Player != null && Bb.Player.IsAlive)
            {
                var pc = Bb.Player.Controller as PlayerController;
                if (pc.Drone.Bb.Targets.Count != 0)
                    targets = pc.Drone.Bb.Targets;
            }
            if (targets == null)
                return null;

            var minDis = float.MaxValue;
            Character minTarget = null;
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i] == null || !targets[i].IsAlive)
                {
                    targets.RemoveAt(i);
                    continue;                    
                }
                var dis = targets[i].Trans.position - Bb.Player.Trans.position;
                if (dis.sqrMagnitude < minDis)
                {
                    minDis = dis.sqrMagnitude;
                    minTarget = targets[i];
                }
            }
            return minTarget;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Fsm?.OnDrawGizmos(transform.position);
        }
#endif
    }
}