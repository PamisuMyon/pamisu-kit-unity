using Pamisu.CustomPP.Effects;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pamisu.CustomPP
{
    public class AwakePerformer : MonoBehaviour
    {
        public Volume Volume;

        [Range(0, 1f)]
        public float Progress = 1f;

        private Awake awake;
        private bool isEnabled = false;

        private void Start()
        {
            Volume.sharedProfile.TryGet(out awake);
            Debug.Assert(awake, $"Awake VolumeComponent must be added to the volume {Volume.gameObject.name}");
        }

        private void Update()
        {
            if (!isEnabled) return;
            awake.Progress.value = Progress;
        }

        // Called by animator
        public void Enable()
        {
            isEnabled = true;
        }

        // Called by animator
        public void Disable()
        {
            isEnabled = false;
        }
        
    }
}