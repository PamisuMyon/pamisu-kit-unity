using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "SprayAbilityConfig", menuName = "Configs/SprayAbilityConfig", order = 0)]
    public class SprayAbilityConfig : AbilityConfig
    {
        [Header("SprayAbilityConfig")]
        public float Length;
        public float DamageScale = 1;

        public EffectType EffectType;
        
        [Space]
        public float MoveSpeedMultiplier;

    }
}