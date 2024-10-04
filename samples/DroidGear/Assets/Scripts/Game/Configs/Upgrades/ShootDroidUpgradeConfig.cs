using Game.Framework;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "ShootDroidUpgrade", menuName = "Configs/Upgrades/ShootDroidUpgradeConfig")]
    public class ShootDroidUpgradeConfig : UpgradeConfig
    {
        
        public int BranchCountAddend;
        public int BurstCountAddend;
        public float ExplosionRadiusMultiplier;
        public float DamageMultiplier;
        
        public override Upgrade CreateUpgrade()
        {
            return new ShootDroidUpgrade(this);
        }
    }
}