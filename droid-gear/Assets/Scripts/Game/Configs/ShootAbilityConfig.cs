using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "UniversalShootAbilityConfig", menuName = "Configs/UniversalShootAbilityConfig", order = 0)]
    public class ShootAbilityConfig : AbilityConfig
    {
        [Header("ShootAbility")]
        public EmitterConfig Emitter;
    }

    public class EmitterConfig
    {
        public int BranchCount = 1;
        public float BranchAngleDelta;
        public int BurstCount = 1;
        public float BurstInterval;
        public ProjectileConfig Projectile;
    }

    public class ProjectileConfig
    {
        public int PierceCount;
        public bool IsExplosion;
        public float ExplosionRange;
    }

}
