using Game.Common;
using UnityEngine;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Track : Base
        {
            private float _trackCounter;
            private float _stoppingDistance;
            private float _stoppingDistanceSqr;

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

                _stoppingDistance = Owner.Chara.Model.VisualRadius + Bb.Target.Model.VisualRadius;
                if (Bb.AttackAbility != null)
                    _stoppingDistance += Bb.AttackAbility.Config.ActRange;
                _stoppingDistanceSqr = _stoppingDistance * _stoppingDistance;
                Owner.Agent.stoppingDistance = _stoppingDistance;
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
                var dir = Bb.Target.Trans.position - Owner.Trans.position;
                if ((Owner.Agent.hasPath 
                    && !Owner.Agent.pathPending 
                    && Owner.Agent.remainingDistance < Owner.Agent.stoppingDistance + 0.1f)
                    || dir.sqrMagnitude < _stoppingDistanceSqr)
                {
                    Machine.ChangeState<Attack>();
                    return;
                }

                if (_trackCounter <= 0)
                {
                    Owner.Agent.destination = Bb.Target.Trans.position;
                    _trackCounter = Owner.TrackFrequency;
                }
                else
                {
                    _trackCounter -= deltaTime;
                }

                Owner.Model.Anim.SetBool(AnimConst.IsRunning, Owner.Agent.velocity != Vector3.zero);
            }

        }
    }
}