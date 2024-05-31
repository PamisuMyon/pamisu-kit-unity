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

namespace Game.Characters
{
    public class DroidController : Framework.CharacterController, IUpdatable
    {
        [SerializeField]
        private TriggerArea _senseArea;

        public float TrackFrequency = .2f;
        public float TurnSpeed = 720f;
        public Vector2 WanderRange = new(1f, 5f);

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
            if (Chara.AbilityComp.TryGetAbility(Config.AttackAbility.Id, out var attackAbility))
                Bb.AttackAbility = attackAbility;
            UniTask.Action(async () => 
            {
                await UniTask.Yield();
                Bb.Player = FindFirstObjectByType<PlayerController>().Chara;
            })();

            _senseArea.TriggerEnter += OnSenseAreaEnter;
            _senseArea.TriggerExit += OnSenseAreaExit;

            Fsm = new StateMachine();
            Fsm.AddState(new DroidStates.Idle(this));
            Fsm.AddState(new DroidStates.Wander(this));
            Fsm.AddState(new DroidStates.Track(this));
            Fsm.AddState(new DroidStates.Attack(this));
            Fsm.AddState(new DroidStates.Death(this));
            Fsm.ChangeState<DroidStates.Idle>();
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
                if (Bb.Target == character) 
                {
                    Bb.Target = null;
                }
            }
        }

        internal bool SelectTarget()
        {
            if (Bb.Targets.Count == 0)
                return false;
            var minDis = float.MaxValue;
            var minTarget = Bb.Targets[0];
            for (int i = 0; i < Bb.Targets.Count; i++)
            {
                var dis = Bb.Targets[i].Trans.position - Chara.Trans.position;
                if (dis.sqrMagnitude < minDis)
                {
                    minDis = dis.sqrMagnitude;
                    minTarget = Bb.Targets[i];
                }
            }
            Bb.Target = minTarget;
            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Fsm?.OnDrawGizmos(transform.position);
        }
#endif
    }
}