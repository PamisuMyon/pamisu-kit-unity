
using UnityEngine;

namespace Game.Common
{
    public class ParticleGroup : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _rootParticles;

        public void Play()
        {
            for (int i = 0; i < _rootParticles.Length; i++)
            {
                _rootParticles[i].Play();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _rootParticles.Length; i++)
            {
                _rootParticles[i].Clear();
            }
        }

    }
}