using UnityEngine;

namespace Game.Combat
{
    public class EnemyStart : MonoBehaviour
    {
        public float SpawnRadius = 5f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.33f, 0f, .5f);
            Gizmos.DrawSphere(transform.position, SpawnRadius);
        }
    }
}