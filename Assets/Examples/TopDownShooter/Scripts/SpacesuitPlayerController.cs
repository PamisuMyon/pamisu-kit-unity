using System;
using Pamisu.Commons.Pool;
using Pamisu.Game;
using UnityEngine;

namespace Pamisu.Examples.TopDownShooter
{
    public class SpacesuitPlayerController : TwinStickShooterPlayerControllerBase
    {

        [Header("Spacesuit Player")]
        [Header("Attack")]
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _fireInterval = .5f;
        [SerializeField]
        private GameObject _projectilePrefab;

        private float _fireCounter;
        
        protected override void Update()
        {
            base.Update();
            HandleAttack();
        }

        private void HandleAttack()
        {
            if (!input.Fire1) return;
            if (_fireCounter > 0)
            {
                _fireCounter -= Time.deltaTime;
                return;
            }

            var go = GameObjectPooler.Spawn(_projectilePrefab);
            go.transform.position = _firePoint.position;
            go.transform.forward = _firePoint.forward;
            _fireCounter = _fireInterval;
        }
        
    }
}