using Game.Configs;
using Game.Framework;
using PamisuKit.Common.FSM;
using UnityEngine.AI;
using Game.Characters.Monster.States;
using Game.Combat;
using Game.Characters.Player;
using PamisuKit.Framework;
using UnityEngine;

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
            {
                ReInit();
                return;
            }
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

        public void ReInit()
        {
            Chara.Revive();
            Chara.AbilityComp.ResetAbilities();
            Fsm.ChangeState<MonsterStates.Idle>();
        }

        public void ApplyGrowth(int waveNum)
        {
            var config = Config as MonsterConfig;
            Debug.Assert(config != null, $"Config {Config.name} is not MonsterConfig");
            var maxHealth = config.AttributeDict[AttributeType.MaxHealth] * (1f + (waveNum - 1) * config.HealthGrowth);
            var damage = config.AttributeDict[AttributeType.Damage] * (1f + (waveNum - 1) * config.DamageGrowth);
            Chara.AttrComp.SetValue(AttributeType.MaxHealth, maxHealth);
            Chara.AttrComp.SetValue(AttributeType.Damage, damage);
            Chara.Revive();
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
            if (GetSystem<CombatSystem>().Bb.Player != null)
            {
                Bb.Target = GetSystem<CombatSystem>().Bb.Player.Chara;
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