using System;
using Cinemachine;
using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Platformer2D
{
    public class CameraShaker : SingletonBehaviour<CameraShaker>
    {
        private CinemachineImpulseSource source;
        
        private void Start()
        {
            source = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(Vector3 velocity)
        {
            source.GenerateImpulse(velocity);
        }
        
    }
}