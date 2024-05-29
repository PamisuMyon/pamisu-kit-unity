using Game.Common;
using UnityEngine;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Track : Base
        {
            internal float UpdateDestinationFrequency;
            private float _updateDestinationCounter;

            public Track(MonsterController owner) : base(owner)
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

                var stoppingDistance = Owner.Chara.Model.VisualRadius + Bb.Target.Model.VisualRadius;
                Owner.Agent.stoppingDistance = stoppingDistance;
                Owner.Agent.isStopped = false;
                Owner.Agent.destination = Bb.Target.Trans.position;
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
                if (Owner.Agent.hasPath 
                    && !Owner.Agent.pathPending 
                    && Owner.Agent.remainingDistance < Owner.Agent.stoppingDistance + 0.1f)
                {
                    Machine.ChangeState<Attack>();
                    return;
                }

                if (_updateDestinationCounter <= 0)
                {
                    Owner.Agent.destination = Bb.Target.Trans.position;
                    _updateDestinationCounter = UpdateDestinationFrequency;
                }
                else
                {
                    _updateDestinationCounter -= deltaTime;
                }

                Owner.Model.Anim.SetBool(AnimConst.IsRunning, Owner.Agent.velocity != Vector3.zero);
            }

        }
    }
}