using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Common;
using Game.Framework;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Props
{
    public class Projectile : MonoEntity, IFixedUpdatable, IPoolElement
    {
        [SerializeField]
        private float _moveSpeed = 10f;
        [SerializeField]
        private bool _showMuzzle = true;
        [SerializeField]
        private ParticleGroup _muzzlePrefab;
        [SerializeField]
        private ParticleGroup _explosionPrefab;

        [Space]
        [SerializeField]
        private GameObject _body;
        [SerializeField]
        private ParticleSystem[] _otherParts;
        [SerializeField]
        private float _otherPartsRecycleDelay = 1f;

        private Rigidbody rb;
        private ParticleGroup _muzzle;
        private ParticleGroup _explosion;
        private Damage _damage;
        private bool _isHit;

        protected override void OnCreate()
        {
            base.OnCreate();
            rb = GetComponent<Rigidbody>();
        }
        
        public void OnFixedUpdate(float deltaTime)
        {
            if (_isHit)
                rb.velocity = Vector3.zero;
            else
                rb.velocity = _moveSpeed * Region.Ticker.TimeScale * Trans.forward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isHit) return;
            if (other.isTrigger) return;
            var hitPos = Trans.position;
            if (other.TryGetComponentInDirectParent<Character>(out var target))
            {
                DamageHelper.ApplyDamage(_damage, target);
            }

            Hit(hitPos).Forget();
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            gameObject.SetActive(false);
        }

        public void Activate(Damage damage, Vector3 position, Vector3 direction, int layer)
        {
            _isHit = false;
            _damage = damage;
            Go.layer = layer;
            Trans.position = position;
            Trans.forward = direction;
            _body.SetActive(true);

            if (_showMuzzle && _muzzlePrefab != null)
            {
                if (_muzzle == null)
                    _muzzle = Instantiate(_muzzlePrefab, Trans.parent);
                _muzzle.transform.position = Trans.position;
                _muzzle.Clear();
                _muzzle.Play();
            }
        }

        private async UniTaskVoid Hit(Vector3 explodePosition)
        {
            _isHit = true;
            if (_explosionPrefab != null)
            {
                if (_explosion == null)
                    _explosion = Instantiate(_explosionPrefab, Trans.parent);
                _explosion.transform.position = explodePosition;
                _explosion.Clear();
                _explosion.Play();
            }

            _body.SetActive(false);
            await Region.Ticker.Delay(_otherPartsRecycleDelay, destroyCancellationToken);
            for (int i = 0; i < _otherParts.Length; i++)
            {
                _otherParts[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            GetDirector<GameDirector>().Pooler.Release(this);
        }

    }
    
}