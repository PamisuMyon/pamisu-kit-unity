using PamisuKit.Common.FSM;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public abstract class Base : State
        {
            protected MonsterController Owner;
            protected Blackboard Bb => Owner.Bb;

            protected Base(MonsterController owner)
            {
                Owner = owner;
            }
        }
    }
}