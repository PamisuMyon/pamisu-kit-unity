using UnityEngine;

namespace Pamisu.Commons.Pool
{
    public class ParticlesRecycleOnCondition : RecycleOnCondition
    {

        public override void OnRecycle()
        {
            var particles = GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            base.OnRecycle();
        }
    }
}