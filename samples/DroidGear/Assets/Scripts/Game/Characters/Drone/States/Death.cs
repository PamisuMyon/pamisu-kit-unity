using LitMotion;
using LitMotion.Extensions;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {
        public class Death : Base
        {
            public Death(DroneController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Fall();
            }

            private void Fall()
            {
                var localY = Owner.Trans.localPosition.y;
                LMotion.Create(localY, .1f, .3f)
                    .BindToPositionY(Owner.Trans);                
            }

        }
    }
}
