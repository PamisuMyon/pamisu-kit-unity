using Game.Common;
using Game.Events;
using PamisuKit.Common;
using UnityEngine;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public class Idle : Base
        {
            private float _idleCounter;

            public Idle(DroidController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.Agent.isStopped = true;
                Owner.Model.Anim.SetBool(AnimConst.IsRunning, false);
                _idleCounter = Random.Range(0, 3f);
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);

                var target = Owner.SelectTarget();
                if (target != null) 
                {
                    Bb.Target = target;
                    Bb.ShouldReturnToPlayer = false;
                    EventBus.Emit(new EnemySpotted(Owner.Chara, target));
                    Machine.ChangeState<Track>();
                    return;
                }

                if (Bb.Player != null && Bb.Player.IsAlive) 
                {
                    var dir = Bb.Player.Trans.position - Owner.Trans.position;
                    if (dir.sqrMagnitude > Owner.DroidConfig.MaxFollowRadius * Owner.DroidConfig.MaxFollowRadius)
                    {
                        Bb.ShouldReturnToPlayer = true;
                        Machine.ChangeState<Track>();
                        return;
                    }
                }

                if (_idleCounter <= 0)
                {
                    Machine.ChangeState<Wander>();
                    return;
                }
                else 
                {
                    _idleCounter -= deltaTime;
                }

            }

        }
    }
}
