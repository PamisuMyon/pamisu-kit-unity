
using UnityEngine;

namespace Game.Configs
{

    public enum AbilityType
    {
        None,
        SimpleShoot,
        SimpleMelee,
    }

    [CreateAssetMenu(fileName = "AbilityConfig", menuName = "Configs/AbilityConfig", order = 0)]
    public class AbilityConfig : ScriptableObject
    {
        [Header("Ability")]
        public string Id;
        public AbilityType Type;
        public float Cooldown;
        public float ActPreDelay;
        public float ActPostDelay;
    }
}
