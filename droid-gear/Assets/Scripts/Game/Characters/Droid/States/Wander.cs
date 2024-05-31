using Game.Common;
using Game.Framework;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public class Wander : Base
        {
            public Wander(DroidController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                if (RandomUtil.RandomPositionOnNavMesh(Owner.Trans.position, Owner.WanderRange.x, Owner.WanderRange.y, out var point))
                {
                    Owner.Agent.destination = point;
                }
                else
                    Machine.ChangeState<Idle>();
            }

            public override void OnExit()
            {
                base.OnExit();
                if (!Owner.IsPendingDestroy && Owner.Agent.enabled)
                    Owner.Agent.isStopped = true;
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime); 
                if (Owner.SelectTarget()) 
                {
                    Bb.ShouldReturnToPlayer = false;
                    Machine.ChangeState<Track>();
                    return;
                }

                var timescale = Owner.Region.Ticker.TimeScale;
                Owner.Agent.speed = Owner.Chara.AttrComp[AttributeType.MoveSpeed].Value * timescale;
                Owner.Agent.angularSpeed = Owner.TurnSpeed * timescale;

                if (Owner.Agent.hasPath 
                    && !Owner.Agent.pathPending 
                    && Owner.Agent.remainingDistance < Owner.Agent.stoppingDistance + 0.1f)
                {
                    Machine.ChangeState<Attack>();
                    return;
                }

                var dir = Bb.Player.Trans.position - Owner.Trans.position;
                if (dir.sqrMagnitude > Owner.DroidConfig.MaxFollowRadius * Owner.DroidConfig.MaxFollowRadius)
                {
                    Bb.ShouldReturnToPlayer = true;
                    Machine.ChangeState<Track>();
                    return;
                }

                Owner.Model.Anim.SetBool(AnimConst.IsRunning, Owner.Agent.velocity != Vector3.zero);
            }

        }
    }
}
