using Game.Framework;
using UnityEngine;
using PamisuKit.Common.Util;
using Cysharp.Threading.Tasks;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {
        public class Attack : Base
        {
            private bool _isReadyForAttack = false;
            private float _targetingCounter;

            public Attack(DroneController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                if (Bb.Target == null || !Bb.Target.IsAlive)
                {
                    Machine.ChangeState<Idle>();
                    return;
                }
                Owner.AttackAbility.SetTarget(new AbilityTargetInfo { MainTarget = Bb.Target });
                _isReadyForAttack = false;
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
                Owner.UpdatePosition();

                var b = Owner.Trans.SmoothRotateTowards(dir, deltaTime, Owner.TurnSpeed);
                if (!_isReadyForAttack && b)
                {
                    _targetingCounter = Owner.TargetingFrequency;
                    _isReadyForAttack = true;
                }

                if (_isReadyForAttack) 
                {
                    _targetingCounter -= deltaTime;
                    if (!Owner.AttackAbility.CanActivate())
                        return;
                    if (CanSeeTarget())
                        PerformAttack().Forget();
                }
            }

            private async UniTaskVoid PerformAttack() 
            {
                await Owner.AttackAbility.Activate(Owner.destroyCancellationToken);
                if (Bb.Targets == null || !Bb.Target.IsActive)
                {
                    Machine.ChangeState<Idle>();
                    return;
                }

                if (_targetingCounter < 0)
                {
                    _targetingCounter = Owner.TargetingFrequency;
                    var target = Owner.SelectTarget();
                    if (target == null || target == Bb.Target)
                        return;
                    Bb.Target = target;
                    Owner.AttackAbility.SetTarget(new AbilityTargetInfo { MainTarget = Bb.Target });
                }
            }

            private bool CanSeeTarget()
            {
                var targetPos = Bb.Target.Trans.position;
                targetPos.y += Bb.Target.Model.VisualHeight;
                var selfPos = Owner.Trans.position;
                selfPos.y += Owner.Model.VisualHeight;

                var b = Physics.Linecast(selfPos, targetPos, Owner.ObstacleLayer);
#if UNITY_EDITOR
                Debug.DrawLine(selfPos, targetPos, b ? Color.yellow : Color.gray, 0.5f);
#endif
                return !b;
            }

        }
        
    }
}
