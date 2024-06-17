using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "ShootAbilityConfig", menuName = "Configs/ShootAbilityConfig", order = 0)]
    public class ShootAbilityConfig : AbilityConfig
    {
        public enum MultiEmitMethod
        {
            Sequence, Parallel
        }

        [Header("ShootAbility")]
        public MultiEmitMethod EmitMethod = MultiEmitMethod.Sequence;
        public EmitterConfig[] Emitters;
    }

    [Serializable]
    public class EmitterConfig
    {
        public bool IsPlayAnim = true;
        public string AnimTriggerParam;
        [Space]
        [Min(0)]
        public int FirePointIndex;
        [Min(1)]
        public int BranchCount = 1;
        public float BranchAngleDelta;
        [Space]
        [Min(1)]
        public int BurstCount = 1;
        public float BurstPreDelay;
        public float BurstPostDelay;
        public float BurstInterval;
        [Space]
        public ProjectileConfig Projectile;
    }

    [Serializable]
    public class ProjectileConfig
    {
        public AssetReferenceGameObject PrefabRef;
        public float MoveSpeed = 10f;
        public int PierceCount;
        public bool IsExplosion;
        public float ExplosionRange;
    }

}
