using System.Collections;
using Pamisu.Commons.Pool;
using Pamisu.TopDownShooter.Enemies;
using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private float initialDamage = 10f;
        [SerializeField]
        private GameObject muzzlePrefab;
        [SerializeField]
        private bool showMuzzle = true;
        [SerializeField]
        private GameObject explosionPrefab;

        [Space]
        [SerializeField]
        private GameObject body;
        [SerializeField]
        private ParticleSystem[] otherParts;
        [SerializeField]
        private float otherPartsRecycleDelay = 1f;

        [Header("View Only")]
        [SerializeField]
        private bool isExploded;
        
        private Rigidbody rb;
        private float damage;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (isExploded)
                rb.velocity = Vector3.zero;
            else
                rb.velocity = transform.forward * speed;
        }

        public void Spawn(Vector3 position, Quaternion rotation, int layer)
        {
            gameObject.layer = layer;
            transform.position = position;
            transform.rotation = rotation;
            
            body.SetActive(true);
            damage = initialDamage;
            isExploded = false;
            
            if (showMuzzle)
                GameObjectPooler.Spawn(muzzlePrefab, position, rotation);
        }

        public void Release()
        {
            StartCoroutine(DoRelease());
        }

        private IEnumerator DoRelease()
        {
            body.SetActive(false);
            yield return new WaitForSeconds(otherPartsRecycleDelay);
            foreach (var it in otherParts)
            {
                it.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            GameObjectPooler.Release(gameObject);
        }

        private void Explode(Vector3 position)
        {
            isExploded = true;
            GameObjectPooler.Spawn(explosionPrefab, position, transform.rotation);
            Release();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isExploded) return;
            if (other.isTrigger) return;
            var explodePos = transform.position;
            
            var attr = other.GetComponentInParent<ActorAttributes>();
            if (attr != null)
            {
                attr.HealthChange(-damage);
                if (attr.gameObject.CompareTag("Enemy"))
                {
                    var halfHeight = attr.GetComponent<EnemyController>().ModelHalfHeight;
                    explodePos.y = attr.transform.position.y + halfHeight;
                }
            }
            Explode(explodePos);
            // Debug.Log("Other " + other.gameObject);
        }
        
    }
}
