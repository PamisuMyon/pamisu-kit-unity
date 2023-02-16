using System.Collections;
using DG.Tweening;
using Pamisu.Commons;
using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.TopDownShooter.Player
{
    public class GearCannon : MonoBehaviour
    {

        [SerializeField]
        private float spawnRadius = 3f;
        [SerializeField]
        private float selfRadius = 1f;
        [SerializeField]
        private LayerMask blockLayers;
        [SerializeField]
        private float spawnHeight = 20f;
        [SerializeField]
        private Transform head;

        [Space]
        [SerializeField]
        private float shootInterval = .2f;
        [SerializeField]
        private float turnSpeed = 1440f;
        [SerializeField]
        private float phaseOneDuration = 5f;
        [SerializeField]
        private float phaseTwoDuration = 6f;
        [SerializeField]
        private Transform[] firePoints;
        [SerializeField]
        private GameObject projectilePrefab;

        [Space]
        [SerializeField]
        private float explosionRadius = 2f;
        [SerializeField]
        private float explosionDamage = 20f;
        [SerializeField]
        private GameObject explosionPrefab;
        
        public void Spawn(Transform instigator)
        {
            transform.rotation = instigator.rotation;
            
            Vector3 position = instigator.position;
            for (var i = 0; i < 10; i++)
            {
                var ann = RandomUtil.InsideAnnulus(selfRadius, spawnRadius);
                position = instigator.transform.position + new Vector3(ann.x, 0, ann.y);
                if (!Physics.CheckSphere(position, selfRadius, blockLayers))
                    break;
            }
            
            var pos = position;
            pos.y = spawnHeight;
            transform.position = pos; 
            transform.DOMove(position, 1f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    StartCoroutine(Perform());
                });
        }

        private IEnumerator Perform()
        {
            StartCoroutine(DoShoot());
            
            var counter = phaseOneDuration;
            var changeDirectionCounter = 0f;

            var targetRotation = head.transform.rotation;
            while (counter > 0)
            {
                if (changeDirectionCounter > 0)
                {
                    changeDirectionCounter -= Time.deltaTime;
                }
                else
                {
                    targetRotation = RandomUtil.RandomYRotation();
                    changeDirectionCounter = RandomUtil.RandomNum(1f, .1f);
                }
                head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                counter -= Time.deltaTime;
                yield return null;
            }
            
            counter = phaseTwoDuration;
            var rotateSpeed = 0f;
            while (counter > 0)
            {
                rotateSpeed += 180 * Time.deltaTime;
                shootInterval -= .08f * Time.deltaTime;
                shootInterval = Mathf.Max(shootInterval, 0.036f);
                head.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
                
                counter -= Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        private IEnumerator DoShoot()
        {
            while (true)
            {
                foreach (var it in firePoints)
                {
                    var go = GameObjectPooler.Spawn(projectilePrefab);
                    var projectile = go.GetComponent<Projectile>();
                    projectile.Spawn(it.position, head.rotation, gameObject.layer);
                }
                yield return new WaitForSeconds(shootInterval);
            }
        }

        private void Explode()
        {
            var cols = Physics.OverlapSphere(head.position, explosionRadius, blockLayers, QueryTriggerInteraction.Ignore);
            foreach (var it in cols)
            {
                var attr = it.GetComponentInParent<ActorAttributes>();
                if (attr != null)
                {
                    attr.HealthChange(-explosionDamage);
                }
            }

            var pos = transform.position;
            pos.y = 1f;
            GameObjectPooler.Spawn(explosionPrefab, pos, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}