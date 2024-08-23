using Game.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    public enum AbilityType
    {
        None,
        SimpleShoot,
        SimpleMelee,
        Shoot,
        Spray,
    }

    [CreateAssetMenu(fileName = "AbilityConfig", menuName = "Configs/AbilityConfig", order = 0)]
    public class AbilityConfig : ScriptableObject
    {
        [Header("Ability")]
        public string Id;
        public AbilityType Type;
        public float Cooldown;
        public float ActRange;
        public float ActPreDelay;
        public float ActPostDelay;
        [Layer]
        public int ActLayer;
        public AssetReferenceGameObject PrefabRef;
    }

}
