using PamisuKit.Common.FSM;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public abstract class Base : State
        {
            protected DroidController Owner;
            protected Blackboard Bb => Owner.Bb;

            protected Base(DroidController owner)
            {
                Owner = owner;
            }
        }
    }
}