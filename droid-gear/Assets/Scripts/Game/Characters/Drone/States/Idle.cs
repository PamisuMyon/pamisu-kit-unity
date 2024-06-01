using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {

        public class Idle : Base
        {
            private float _idleTime = 1f;
            private Quaternion _idleRotation;

            public Idle(DroneController owner) : base(owner)
            {
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);

                if (Owner.SelectTarget()) 
                {
                    Machine.ChangeState<Attack>();
                    return;
                }

                Owner.CurrentAngle += Owner.OrbitSpeed * deltaTime;
                if (Owner.CurrentAngle > 360f)
                    Owner.CurrentAngle -= 360f;

                if (_idleTime > 0)
                {
                    _idleTime -= deltaTime;
                    if (_idleTime <= 0)
                        _idleRotation = RandomUtil.RandomYRotation();
                }
                else
                {
                    var rotation = Owner.transform.rotation;
                    if (rotation.Approximately(_idleRotation))
                    {
                        _idleTime = Random.Range(0f, 3f);   // hard-code
                    }
                    else
                    {
                        Owner.transform.rotation = Quaternion.RotateTowards(rotation, _idleRotation, Owner.OrbitSpeed * 2f * deltaTime);
                    }
                }

                Owner.UpdatePosition();
            }
        }
    }
}
