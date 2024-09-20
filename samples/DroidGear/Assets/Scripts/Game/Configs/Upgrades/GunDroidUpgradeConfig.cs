using Game.Framework;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "GunDroid_", menuName = "Configs/Upgrades/GunDroidUpgradeConfig", order = 0)]
    public class GunDroidUpgradeConfig : UpgradeConfig
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