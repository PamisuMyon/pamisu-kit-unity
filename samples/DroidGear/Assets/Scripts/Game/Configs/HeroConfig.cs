using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/HeroConfig", order = 101)]
    public class HeroConfig : CharacterConfig
    {
        [Header("Hero")]
        public CharacterConfig DroneConfig;
    }
}
