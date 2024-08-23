using System.Collections.Generic;
using Game.Framework;
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
        public float AttackSpeedMultiplier;
        
        public Dictionary<AttributeType, float> AttributeDict = new();
        
        protected virtual void OnEnable()
        {
            Init();
        }

        public virtual void Init()
        {
            AttributeDict.Clear();
            AttributeDict[AttributeType.MoveSpeed] = MoveSpeedMultiplier;
            AttributeDict[AttributeType.AttackSpeed] = AttackSpeedMultiplier;
        }

    }
}