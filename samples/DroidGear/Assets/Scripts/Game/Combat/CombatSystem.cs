using Game.Combat.States;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.FSM;
using PamisuKit.Framework;

namespace Game.Combat
{
    public class CombatSystem : MonoSystem<CombatSystem>
    {
        public LevelConfig LevelConfig;
        public string PlayerId;
        public string DroidId;

        public CombatStates.Blackboard Bb { get; private set; }
        public StateMachine Fsm { get; private set;}
        public CameraController Cam { get; internal set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Cam = FindFirstObjectByType<CameraController>();
            Bb = new CombatStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new CombatStates.Begin(this));
            Fsm.AddState(new CombatStates.Battle(this));
            Fsm.AddState(new CombatStates.End(this));
        }

        public void StartCombat()
        {
            Fsm.ChangeState<CombatStates.Begin>();
        }

    }
}