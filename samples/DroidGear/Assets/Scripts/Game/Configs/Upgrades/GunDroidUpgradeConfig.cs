using Game.Framework;
using Game.Modifiers;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "GunDroid_", menuName = "Configs/Upgrades/GunDroidUpgradeConfig", order = 0)]
    public class GunDroidUpgradeConfig : GearUpgradeConfig
    {

        public int BranchCountAddend;
        public int BurstCountAddend;
        public float DamageMultiplier;
        
        public override Upgrade CreateUpgrade()
        {
            return new GunDroidUpgrade(this);
        }
    }
}