using Game.Framework;
using UnityEngine;

namespace Game.Configs
{
    public enum EffectType
    {
        None,
        Cold,
        Freeze,
    }
    
    [CreateAssetMenu(fileName = "EffectConfig", menuName = "Configs/EffectConfig", order = 0)]
    public class EffectConfig : ScriptableObject
    {
        [Header("Effect")]
        public EffectType Type;
        public EffectDurationPolicy DurationPolicy;
        public float Duration;
        public float Period;
        public bool PeriodExecuteWhenApply;
    }
}