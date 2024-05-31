using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "MonsterConfig", menuName = "Configs/MonsterConfig", order = 103)]
    public class MonsterConfig : CharacterConfig
    {
        [Header("Monster")]
        public float HealthGrowth;
        public float DamageGrowth;
    }
}
