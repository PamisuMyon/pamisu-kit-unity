using Game.Framework;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "ShootAbilityUpgrade", menuName = "Configs/Upgrades/ShootAbilityUpgradeConfig", order = 0)]
    public class ShootAbilityUpgradeConfig : UpgradeConfig
    {
        
        public int BranchCountAddend;
        public int BurstCountAddend;
        public float ExplosionRadiusMultiplier;
        public float DamageMultiplier;
        
        public override Upgrade CreateUpgrade()
        {
            return new ShootAbilityUpgrade(this);
        }
    }
}