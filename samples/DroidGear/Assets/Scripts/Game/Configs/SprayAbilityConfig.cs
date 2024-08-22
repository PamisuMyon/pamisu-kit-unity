using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "SprayAbilityConfig", menuName = "Configs/SprayAbilityConfig", order = 0)]
    public class SprayAbilityConfig : AbilityConfig
    {
        [Header("SprayAbilityConfig")]
        public float Duration;
        public float DamageScale = 1;
        public EffectConfig Effect;
        
        [Space]
        public float MoveSpeedMultiplier;

    }
}