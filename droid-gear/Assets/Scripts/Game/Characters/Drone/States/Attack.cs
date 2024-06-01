using Game.Framework;
using UnityEngine;
using PamisuKit.Common.Util;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {
        public class Attack : Base
        {

            private bool _isReadyForAttack = false;

            public Attack(DroneController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.AttackAbility.SetTarget(new AbilityTargetInfo { MainTarget = Bb.Target });
            }

            public override void OnExit()
            {
                base.OnExit();
                Owner.AttackAbility.ClearTarget();
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);
                if (Bb.Target == null || !Bb.Target.IsAlive)
                {
                    Bb.Target = null;
                    Machine.ChangeState<Idle>();
                    return;
                }

                var dir = Bb.Target.Trans.position - Owner.Owner.Trans.position;
                var targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                Owner.CurrentAngle = Mathf.MoveTowardsAngle(Owner.CurrentAngle, targetAngle, Owner.OrbitSpeed * 2f * deltaTime);
                var b = Owner.Trans.SmoothRotateTowards(dir, deltaTime, Owner.OrbitSpeed * 2f);
                if (!_isReadyForAttack && b)
                    _isReadyForAttack = true;

                if (_isReadyForAttack)
                    Owner.PerformAttack();

                Owner.UpdatePosition();
            }
        }
        
    }
}
