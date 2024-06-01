using Game.Configs;
using Game.Framework;
using PamisuKit.Common.FSM;
using UnityEngine.AI;
using Game.Characters.Monster.States;
using Game.Combat;
using Game.Characters.Player;
using PamisuKit.Framework;

namespace Game.Characters.Monster
{
    public class MonsterController : Framework.CharacterController, IUpdatable
    {
        public float TrackFrequency = .2f;
        public float TurnSpeed = 720f;

        public NavMeshAgent Agent { get; private set; }
        public StateMachine Fsm { get; private set; }
        public MonsterStates.Blackboard Bb { get; private set; }
        

        public override void Init(CharacterConfig config)
        {
            if (IsInitiated)
                return;
            base.Init(config);

            Agent = GetComponent<NavMeshAgent>();

            Bb = new MonsterStates.Blackboard();
            if (Chara.AbilityComp.TryGetAbility(Config.AttackAbility.Id, out var attackAbility))
                Bb.AttackAbility = attackAbility;

            Fsm = new StateMachine();
            Fsm.AddState(new MonsterStates.Idle(this));
            Fsm.AddState(new MonsterStates.Track(this));
            Fsm.AddState(new MonsterStates.Attack(this));
            Fsm.AddState(new MonsterStates.Death(this));
            Fsm.ChangeState<MonsterStates.Idle>();
        }

        protected override void OnSelfDestroy()
        {
            Fsm?.OnDestroy();
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm?.OnUpdate(deltaTime);
        }

        public void SelectTarget()
        {
            if (CombatSystem.Instance.Bb.Player != null)
            {
                Bb.Target = CombatSystem.Instance.Bb.Player.Chara;
            }
            else 
            {
                var pc = FindFirstObjectByType<PlayerController>();
                if (pc != null)
                    Bb.Target = pc.Chara;
            }
        }

        protected override void OnDie(Character character)
        {
            if (Fsm.CurrentState is not MonsterStates.Death) 
            {
                Fsm.ChangeState<MonsterStates.Death>();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Fsm?.OnDrawGizmos(transform.position);
        }
#endif
    }
}