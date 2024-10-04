
using Cysharp.Threading.Tasks;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Common
{
    public class ParticleGroup : MonoBehaviour, IPoolElement
    {
        public float Duration = 3f;
        [SerializeField]
        private ParticleSystem[] _rootParticles;

        public async UniTaskVoid PlayAndRelease(Ticker ticker, MonoPooler pooler)
        {
            Play();
            await ticker.Delay(Duration, destroyCancellationToken);
            Stop();
            pooler.Release(this);
        }

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

        public void Stop(bool withChildren = true, ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmittingAndClear)
        {
            for (int i = 0; i < _rootParticles.Length; i++)
            {
                _rootParticles[i].Stop(withChildren, stopBehavior);
            }
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            gameObject.SetActive(false);
        }
    }
}