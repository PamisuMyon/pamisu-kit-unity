using System.Collections.Generic;
using Game.Framework;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Characters.Player
{
    public class PlayerDrone : MonoBehaviour, IUpdatable
    {

        [SerializeField]
        private float _orbitSpeed = 180f;

        [SerializeField]
        private float _orbitRadius = .7f;

        private float _currentAngle;
        private float _idleTime = 1f;
        private Quaternion _idleRotation;

        public bool IsActive => Go.activeInHierarchy;
        public GameObject Go { get; private set; }
        public Character Target { get; internal set; }
        public Character Owner { get; internal set; }

        private void Awake()
        {
            Go = gameObject;
            var offset = transform.localPosition;
            _currentAngle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
        }

        public void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        public void OnUpdate(float deltaTime)
        {
            if (Target == null)
            {
                _currentAngle += _orbitSpeed * deltaTime;
                if (_currentAngle > 360f)
                    _currentAngle -= 360f;

                if (_idleTime > 0)
                {
                    _idleTime -= deltaTime;
                    if (_idleTime <= 0)
                        _idleRotation = RandomUtil.RandomYRotation();
                }
                else
                {
                    var rotation = transform.rotation;
                    if (rotation.Approximately(_idleRotation))
                    {
                        _idleTime = Random.Range(0f, 3f);   // hard-code
                    }
                    else
                    {
                        transform.rotation = Quaternion.RotateTowards(rotation, _idleRotation, _orbitSpeed * 2f * deltaTime);
                    }
                }
            }
            else 
            {
                if (!Target.IsAlive)
                {
                    Target = null;
                    return;
                }
                var dir = Target.Trans.position - Owner.Trans.position;
                var targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, targetAngle, _orbitSpeed * 2f * deltaTime);
                transform.SmoothRotateTowards(dir, deltaTime, _orbitSpeed * 2f);
            }
            var x = _orbitRadius * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
            var z = _orbitRadius * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
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
            return Target = minTarget;
        }

    }    
}
