
using Game.Events;
using PamisuKit.Common;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class End : Base
        {
            public End(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                EventBus.Emit(new CombatStateChanged(typeof(End)));
            }
        }
    }
}
