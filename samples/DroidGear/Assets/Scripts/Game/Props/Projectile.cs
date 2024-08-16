using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Common;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Props
{
    public class Projectile : MonoEntity, IFixedUpdatable, IPoolElement
    {
        [SerializeField]
        private float _moveSpeed = 10f;
        [SerializeField]
        private float _maxLifetime = 2f;
        [SerializeField]
        private bool _showMuzzle = true;
        [SerializeField]
        private bool _attachMuzzleToFirePoint = false;
        // Preload these assets to prevent lagging
        [SerializeField]
        private AssetReferenceGameObject _muzzleRef;
        [SerializeField]
        private AssetReferenceGameObject _hitRef;

        private Rigidbody _rb;
        private Collider _col;
        private ProjectileModel _model;

        private ProjectileConfig _config;
        private Damage _damage;
        private Transform _firePoint;
        private bool _isHit;
        private bool _isActivated;


        protected override void OnCreate()
        {
            base.OnCreate();
            _rb = GetComponent<Rigidbody>();
            _col = Trans.Find("Collision").GetComponent<Collider>();
            _model = GetComponentInChildren<ProjectileModel>();
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!_isActivated)
                _rb.velocity = Vector3.zero;
            else
                _rb.velocity = _moveSpeed * Region.Ticker.TimeScale * Trans.forward;
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
            _isActivated = false;
        }

        public void OnReleaseToPool()
        {
            gameObject.SetActive(false);
            _isActivated = false;
            _config = null;
            _firePoint = null;
        }

        public Projectile SetData(
            ProjectileConfig config, 
            Damage damage, 
            Vector3 position,
            Vector3 direction,
            int layer,
            Transform firePoint = null)
        {
            _config = config;
            _moveSpeed = config.MoveSpeed;
            _damage = damage;
            Go.layer = layer;
            _col.gameObject.layer = layer;
            Trans.position = position;
            Trans.forward = direction;
            _firePoint = firePoint;
            return this;
        }

        public void Activate()
        {
            _isActivated = true;
            _isHit = false;
            _model.gameObject.SetActive(true);

            LifetimeCountdown().Forget();

            if (_showMuzzle && _muzzleRef != null)
            {
                SpawnAndReleaseParticleGroup(_muzzleRef, Trans.position, Trans.rotation, _attachMuzzleToFirePoint).Forget();
            }
        }

        private async UniTaskVoid Hit(Vector3 explodePosition)
        {
            _isHit = true;
            if (_hitRef != null)
            {
                SpawnAndReleaseParticleGroup(_hitRef, explodePosition, Trans.rotation).Forget();
            }

            _model.gameObject.SetActive(false);
            await Region.Ticker.Delay(_model.TrailsReleaseDelay, destroyCancellationToken);
            for (int i = 0; i < _model.Trails.Length; i++)
            {
                _model.Trails[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            GetDirector<GameDirector>().Pooler.Release(this);
        }

        private async UniTaskVoid SpawnAndReleaseParticleGroup(object key, Vector3 position, Quaternion rotation, bool attachToFirePoint = false)
        {
            var pooler = GetDirector<GameDirector>().Pooler;
            var it = await pooler.Spawn<ParticleGroup>(key, -1, destroyCancellationToken);
            if (attachToFirePoint && _firePoint != null)
                it.transform.SetParent(_firePoint);
            it.transform.SetPositionAndRotation(position, rotation);
            it.PlayAndRelease(Region.Ticker, pooler).Forget();
        }

        private async UniTaskVoid LifetimeCountdown()
        {
            var counter = _maxLifetime;
            while (!_isHit && !destroyCancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield(destroyCancellationToken);
                counter -= Region.Ticker.DeltaTime;
                if (counter <= 0)
                {
                    GetDirector<GameDirector>().Pooler.Release(this);
                    break;
                }
            }
        }

    }

}