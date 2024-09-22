using Game.Framework;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "SprayDroidUpgradeConfig", menuName = "Configs/Upgrades/SprayDroidUpgradeConfig")]
    public class SprayDroidUpgradeConfig : UpgradeConfig
    {

        public float DurationAddend;
        public float RangeMultiplier;
        public float DamageMultiplier;
        
        public override Upgrade CreateUpgrade()
        {
            return new SprayDroidUpgrade(this);
        }
        
    }
}