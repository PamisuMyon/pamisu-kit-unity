using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    
    [CreateAssetMenu(fileName = "ShootAbilityConfig", menuName = "Configs/ShootAbilityConfig", order = 0)]
    public class ShootAbilityConfig : AbilityConfig
    {
        [Header("ShootAbility")]
        public EmitMethod EmitMethod = EmitMethod.Sequence;
        public EmitterConfig[] Emitters;
    }
    
    public enum EmitMethod
    {
        None,
        Sequence, 
        Parallel
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
        public float DamageScale = 1f;
        public int PierceCount;
        
        [Space]
        public bool IsExplosion;
        public float ExplosionRadius;
        internal float ExplosionRadiusMultiplier;
        public LayerMask ExplosionDamageLayerMask;
        
        // Use ISerializationCallbackReceiver or Odin to solve the serializing problem of cyclic reference 
        // [Space] 
        // public EmitMethod EmitMethod = EmitMethod.None;
        // public EmitterConfig[] Emitters;
    }

}
