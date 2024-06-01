using PamisuKit.Common.FSM;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {
        public abstract class Base : State
        {
            protected DroneController Owner;
            protected Blackboard Bb => Owner.Bb;

            protected Base(DroneController owner)
            {
                Owner = owner;
            }
        }
    }
}