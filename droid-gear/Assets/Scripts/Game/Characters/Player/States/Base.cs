using PamisuKit.Common.FSM;

namespace Game.Characters.Player.States
{
    public static partial class PlayerStates
    {
        public abstract class Base : State
        {
            protected PlayerController Owner;
            protected Blackboard Bb => Owner.Bb;

            protected Base(PlayerController owner)
            {
                Owner = owner;
            }
        }
    }
}