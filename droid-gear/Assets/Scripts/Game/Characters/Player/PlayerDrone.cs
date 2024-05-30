using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Framework;
using PamisuKit.Common.FSM;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Characters.Player
{
    public class PlayerDrone : MonoEntity, IUpdatable
    {
        [SerializeField]
        private float _orbitSpeed = 180f;

        [SerializeField]
        private float _orbitRadius = .7f;

        private float _currentAngle;
        private Ability _attackAbility;

        public StateMachine Fsm { get; private set; }
        public Character Owner { get; private set; }
        public Character Target { get; internal set; }
        public bool IsReadyForAttack { get; private set; }

        public void Init(Character owner)
        {
            Owner = owner;
            Owner.AbilityComp.TryGetAbility(Owner.Config.AttackAbility.Id, out _attackAbility);

            var offset = transform.localPosition;
            _currentAngle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
            
            Fsm = new StateMachine();
            Fsm.AddState(new Idle(this));
            Fsm.AddState(new Attack(this));
            Fsm.ChangeState<Idle>();
        }

        public void OnUpdate(float deltaTime)
        {
            Fsm.OnUpdate(deltaTime);
        }

        private void UpdatePosition()
        {
            var x = _orbitRadius * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
            var z = _orbitRadius * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

            Trans.localPosition = new Vector3(x, Trans.localPosition.y, z);
        }

        public Character SelectTarget(List<Character> targets)
        {
            var minDis = float.MaxValue;
            var minTarget = targets[0];
            var position = transform.position;
            for (int i = 0; i < targets.Count; i++)
            {
                var dis = (targets[i].Trans.position - position).sqrMagnitude;
                if (dis < minDis)
                {
                    minDis = dis;
                    minTarget = targets[i];
                }
            }
            Target = minTarget;
            Fsm.ChangeState<Attack>();
            return Target;
        }

        private void PerformAttack()
        {
            if (!_attackAbility.CanActivate())
                return;
            _attackAbility.Activate(destroyCancellationToken).Forget();
        }

        private abstract class BaseState : State
        {
            protected PlayerDrone Owner;

            protected BaseState(PlayerDrone owner)
            {
                Owner = owner;
            }
        }

        private class Idle : BaseState
        {

            private float _idleTime = 1f;
            private Quaternion _idleRotation;

            public Idle(PlayerDrone owner) : base(owner)
            {
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);
                Owner._currentAngle += Owner._orbitSpeed * deltaTime;
                if (Owner._currentAngle > 360f)
                    Owner._currentAngle -= 360f;

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
                        Owner.transform.rotation = Quaternion.RotateTowards(rotation, _idleRotation, Owner._orbitSpeed * 2f * deltaTime);
                    }
                }

                Owner.UpdatePosition();
            }
        }

        private class Attack : BaseState
        {
            public Attack(PlayerDrone owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner._attackAbility.SetTarget(new AbilityTargetInfo { MainTarget = Owner.Target });
            }

            public override void OnExit()
            {
                base.OnExit();
                Owner.IsReadyForAttack = false;
                Owner._attackAbility.ClearTarget();
            }

            public override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);
                if (Owner.Target == null || !Owner.Target.IsAlive)
                {
                    Owner.Target = null;
                    Machine.ChangeState<Idle>();
                    return;
                }

                var dir = Owner.Target.Trans.position - Owner.Owner.Trans.position;
                var targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                Owner._currentAngle = Mathf.MoveTowardsAngle(Owner._currentAngle, targetAngle, Owner._orbitSpeed * 2f * deltaTime);
                var b = Owner.transform.SmoothRotateTowards(dir, deltaTime, Owner._orbitSpeed * 2f);
                if (!Owner.IsReadyForAttack && b)
                    Owner.IsReadyForAttack = b;

                if (Owner.IsReadyForAttack)
                    Owner.PerformAttack();

                Owner.UpdatePosition();
            }
        }

    }
}
