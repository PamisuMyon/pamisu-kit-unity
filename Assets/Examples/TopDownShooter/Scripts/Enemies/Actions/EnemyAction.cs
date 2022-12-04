using System;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{

    [Serializable]
    public abstract class EnemyAction : MonoBehaviour
    {
        public bool CanBeBlocked;
        public float BlockTime;
        public float Cooldown;
        public float MaxTargetDistance;

        public EnemyController Owner { get; set; }
        public float CooldownCounter { get; set; }

        private float maxTargetDistanceSqr;
        
        protected virtual void Start()
        {
            maxTargetDistanceSqr = MaxTargetDistance * MaxTargetDistance;
        }

        public abstract void Perform(Action onCompleted);

        public abstract void Stop();

        public bool IsInCoolDown => CooldownCounter > 0;

        public bool CheckTargetDistanceSqr(float distanceSqr)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (MaxTargetDistance == -1f) return true;
            return distanceSqr <= maxTargetDistanceSqr;
        }
        
    }
}