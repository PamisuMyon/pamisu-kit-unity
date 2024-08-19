using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Common;
using Game.Framework;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Projectiles
{
    public class SimpleProjectile : MonoEntity, IFixedUpdatable, IPoolElement
    {
        [SerializeField]
        private float _moveSpeed = 10f;
        [SerializeField]
        private float _maxLifetime = 2f;
        [SerializeField]
        private bool _showMuzzle = true;
        // Preload these assets to prevent lagging
        [SerializeField]
        private AssetReferenceGameObject _muzzleRef;
        [SerializeField]
        private AssetReferenceGameObject _hitRef;

        private Rigidbody rb;
        private ProjectileModel _model;
        private Damage _damage;
        private bool _isHit;

        protected override void OnCreate()
        {
            base.OnCreate();
            rb = GetComponent<Rigidbody>();
            _model = GetComponentInChildren<ProjectileModel>();
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
            _model.gameObject.SetActive(true);

            LifetimeCountdown().Forget();

            if (_showMuzzle && _muzzleRef != null)
            {
                SpawnAndReleaseParticleGroup(_muzzleRef, Trans.position, Trans.rotation).Forget();
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

        private async UniTaskVoid SpawnAndReleaseParticleGroup(object key, Vector3 position, Quaternion rotation)
        {
            var pooler = GetDirector<GameDirector>().Pooler;
            var it = await pooler.Spawn<ParticleGroup>(key, -1, destroyCancellationToken);
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