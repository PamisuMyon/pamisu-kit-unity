using Game.Configs;
using Game.Framework;
using PamisuKit.Common.FSM;
using UnityEngine;
using UnityEngine.AI;
using Game.Characters.Monster.States;

namespace Game.Characters
{
    public class MonsterController : Framework.CharacterController
    {
        [SerializeField]
        private float _moveSpeed = 5f;

        public float TurnSpeed = 720f;

        public NavMeshAgent Agent { get; private set; }
        public StateMachine Fsm { get; private set; }
        public MonsterStates.Blackboard Bb { get; private set; }

        public override void Init(CharacterConfig config)
        {
            base.Init(config);
            Chara.AttrComp.HealthChanged += OnHealthChanged;

            Agent = GetComponent<NavMeshAgent>();

            Bb = new MonsterStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new MonsterStates.Idle(this));
            Fsm.AddState(new MonsterStates.Track(this));
            Fsm.AddState(new MonsterStates.Attack(this));
            Fsm.AddState(new MonsterStates.Death(this));
            Fsm.ChangeState<MonsterStates.Idle>();

            // SelectTarget();
        }

        protected override void OnSelfDestroy()
        {
            Fsm?.OnDestroy();
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm.OnUpdate(deltaTime);
        }

        public void SelectTarget()
        {

        }

        protected virtual void OnHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            if (newHealth <= 0)
                Fsm.ChangeState<MonsterStates.Death>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Fsm?.OnDrawGizmos(transform.position);
        }
#endif
    }
}