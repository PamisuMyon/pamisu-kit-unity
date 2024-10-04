using PamisuKit.Common.FSM;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public abstract class Base : State
        {
            protected CombatSystem Owner;
            protected Blackboard Bb => Owner.Bb;

            protected Base(CombatSystem owner)
            {
                Owner = owner;
            }
        }
    }
}
