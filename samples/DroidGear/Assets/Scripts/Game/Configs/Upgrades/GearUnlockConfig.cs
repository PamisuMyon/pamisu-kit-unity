using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "GearUnlock_", menuName = "Configs/Upgrades/GearUnlockConfig", order = 0)]
    public class GearUnlockConfig : UpgradeConfig
    {
        public CharacterConfig Chara;
    }
}
