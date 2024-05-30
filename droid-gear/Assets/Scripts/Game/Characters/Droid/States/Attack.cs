using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Framework;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public class Attack : Base
        {
            private Ability _attackAbility;
            private bool _isLookingAtTarget;
            private bool _isPerformingAttack;
            
            public Attack(DroidController owner) : base(owner)
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
                
                Owner.Chara.AbilityComp.TryGetAbility(Owner.Chara.Config.AttackAbility.Id, out _attackAbility);
                Debug.Assert(_attackAbility != null, "Attack ability can not be null", Owner.gameObject);
                _isPerformingAttack = false;
                _isLookingAtTarget = false;
                Owner.Agent.isStopped = true;
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);
                Owner.Model.Anim.SetBool(AnimConst.IsRunning, Owner.Agent.velocity != Vector3.zero);

                if (_isPerformingAttack)
                    return;
                if (!_isLookingAtTarget)
                {
                    var dir = Bb.Target.Trans.position - Owner.Chara.Trans.position;
                    dir.y = 0f;
                    _isLookingAtTarget = Owner.Chara.Trans.SmoothRotateTowards(dir, deltaTime, Owner.TurnSpeed);
                }

                if (_isLookingAtTarget && _attackAbility.CanActivate())
                {
                    PerformAttack().Forget();
                    _isPerformingAttack = true;
                }
            }

            private async UniTaskVoid PerformAttack()
            {
                _attackAbility.SetTarget(new AbilityTargetInfo
                {
                    MainTarget = Bb.Target
                });
                await _attackAbility.Activate();
                _isPerformingAttack = false;

                if (Bb.Target == null || !Bb.Target.IsAlive)
                {
                    Machine.ChangeState<Idle>();
                    return;
                }

                if (Owner.Chara.IsAdjacent(Bb.Target, .2f))
                {
                    // Debug.Log("Target in range, keep attacking", Owner.gameObject);
                    _isLookingAtTarget = false;
                }
                else
                {
                    // Debug.Log("Target not in range, tracking", Owner.gameObject);
                    Machine.ChangeState<Track>();
                    return;
                }
            }
            
        }
    }
}