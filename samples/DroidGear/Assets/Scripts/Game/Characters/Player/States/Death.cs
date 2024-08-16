using Game.Common;

namespace Game.Characters.Player.States
{
    public static partial class PlayerStates
    {
        public class Death : Base
        {
            public Death(PlayerController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.Model.Anim.SetTrigger(AnimConst.Death);
            }

        }
    }
}