using Game.Combat.States;
using Game.Configs;
using Game.Events;
using Game.Framework;
using PamisuKit.Common;
using PamisuKit.Common.FSM;
using PamisuKit.Framework;

namespace Game.Combat
{
    public class CombatSystem : MonoSystem
    {
        public LevelConfig LevelConfig;
        public HeroConfig PlayerConfig;
        public DroidConfig DroidConfig;

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

        public void AddPlayerExp(float delta)
        {
            Bb.Experience += delta;
            // var level = Bb.PlayerLevel;
            var levelUpDelta = 0;
            while (Bb.Experience >= Bb.NextLevelExperience)
            {
                levelUpDelta++;
                // Level up
                Bb.PlayerLevel++;
                Bb.Experience -= Bb.NextLevelExperience;
                float increment;
                if (Bb.PlayerLevel >= 2 && Bb.PlayerLevel < 20)
                    increment = 10;
                else if (Bb.PlayerLevel >= 20 && Bb.PlayerLevel < 40)
                    increment = 13;
                else
                    increment = 16;
                Bb.NextLevelExperience += increment;
            }
            EventBus.Emit(new PlayerExpChanged
            {
                NewLevel = Bb.PlayerLevel,
                LevelUpDelta = levelUpDelta,
                NewExp = Bb.Experience,
                ExpDelta = delta,
                NextLevelExp = Bb.NextLevelExperience
            });
        }
        
    }
}