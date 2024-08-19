using System.Collections.Generic;
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

namespace Game.Projectiles
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
        [Space]
        [SerializeField]
        private AssetReferenceGameObject _muzzleRef;
        [SerializeField]
        private AssetReferenceGameObject _hitRef;
        [SerializeField]
        private float _hitModelRadius;

        private Rigidbody _rb;
        private Collider _col;
        private ProjectileModel _model;

        private ProjectileConfig _config;
        private Damage _damage;
        private Transform _firePoint;
        private Collider[] _overlapResults = new Collider[128];
        private HashSet<Character> _damagedTargets = new();
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
            if (_config.IsExplosion)
            {
                Explode(hitPos).Forget();
            }
            else
            {
                other.TryGetComponentInDirectParent<Character>(out var target);
                Hit(hitPos, target).Forget();
            }
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

            if (!_config.IsExplosion)
                LifetimeCountdown().Forget();

            if (_showMuzzle && _muzzleRef != null)
            {
                SpawnAndReleaseParticleGroup(_muzzleRef, Trans.position, Trans.rotation, Vector3.one, _attachMuzzleToFirePoint).Forget();
            }
        }

        private async UniTaskVoid Hit(Vector3 position, Character target = null)
        {
            _isHit = true;
            if (target != null)
            {
                var damage = _damage;
                damage.Value *= _config.DamageScale;
                DamageHelper.ApplyDamage(damage, target);
            }
            
            if (_hitRef != null)
            {
                SpawnAndReleaseParticleGroup(_hitRef, position, Trans.rotation, Vector3.one).Forget();
            }

            _model.gameObject.SetActive(false);
            
            // await ProcessSubEmitters();
            
            await Region.Ticker.Delay(_model.TrailsReleaseDelay, destroyCancellationToken);
            for (int i = 0; i < _model.Trails.Length; i++)
            {
                _model.Trails[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            
            GetDirector<GameDirector>().Pooler.Release(this);
        }

        private async UniTaskVoid Explode(Vector3 position)
        {
            _isHit = true;

            // Apply damage
            _damagedTargets.Clear();
            var damage = _damage;
            damage.Value *= _config.DamageScale;
            var count = Physics.OverlapSphereNonAlloc(transform.position, _config.ExplosionRadius, _overlapResults, _config.ExplosionDamageLayerMask);
            for (int i = 0; i < count; i++)
            {
                if (_overlapResults[i].TryGetComponentInDirectParent<Character>(out var target))
                {
                    if (!_damagedTargets.Contains(target))
                    {
                        DamageHelper.ApplyDamage(damage, target);
                        _damagedTargets.Add(target);
                    }
                }
            }
            
            // Effects
            if (_hitRef != null)
            {
                var s = _config.ExplosionRadius / _hitModelRadius;
                var scale = new Vector3(s, s, s);
                SpawnAndReleaseParticleGroup(_hitRef, position, Trans.rotation, scale).Forget();
            }

            _model.gameObject.SetActive(false);

            // await ProcessSubEmitters();
            
            await Region.Ticker.Delay(_model.TrailsReleaseDelay, destroyCancellationToken);
            for (int i = 0; i < _model.Trails.Length; i++)
            {
                _model.Trails[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            GetDirector<GameDirector>().Pooler.Release(this);
        }

        private async UniTaskVoid SpawnAndReleaseParticleGroup(
            object key, 
            Vector3 position, 
            Quaternion rotation, 
            Vector3 scale, 
            bool attachToFirePoint = false)
        {
            var pooler = GetDirector<GameDirector>().Pooler;
            var it = await pooler.Spawn<ParticleGroup>(key, -1, destroyCancellationToken);
            if (attachToFirePoint && _firePoint != null)
            {
                it.transform.SetParent(null);
                it.transform.localScale = scale;
                it.transform.SetParent(_firePoint);
            }
            else
                it.transform.localScale = scale;
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

        // private UniTask ProcessSubEmitters()
        // {
        //     return this.ProcessEmitters(_config.Emitters, _config.EmitMethod, _damage, destroyCancellationToken);
        // }

    }

}