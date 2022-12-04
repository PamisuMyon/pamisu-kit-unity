using UnityEngine;

namespace Pamisu.TopDownShooter
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraBounds : MonoBehaviour
    {
        public Bounds WorldBounds { get; private set; }

        private BoxCollider box;

        private void Awake()
        {
            box = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            UpdateWorldBounds();
        }

        public void UpdateWorldBounds()
        {
            var cam = Camera.main;
            if (!cam) return;

        }
    }
}