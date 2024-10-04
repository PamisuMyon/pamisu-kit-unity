using Game.Common;
using UnityEngine;

namespace Game.Characters.Player.States
{
    public static partial class PlayerStates
    {
        public class Normal : Base
        {
            public Normal(PlayerController owner) : base(owner)
            {
            }
            
            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);
                Owner.Model.Anim.SetBool(AnimConst.IsRunning, Owner.Movement != Vector3.zero);
                Owner.HandleOrientation(deltaTime);
            }

            public override void OnFixedUpdate(float deltaTime)
            {
                Owner.HandleMovement();
            }

        }
    }
}
