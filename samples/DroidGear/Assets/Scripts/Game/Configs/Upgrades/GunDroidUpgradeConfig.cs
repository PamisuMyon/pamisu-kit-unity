using Game.Framework;
using Game.Modifiers;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "GunDroid_", menuName = "Configs/Upgrades/GunDroidUpgradeConfig", order = 0)]
    public class GunDroidUpgradeConfig : UpgradeConfig
    {
        
        
        
        public override Upgrade CreateUpgrade()
        {
            return new GunDroidUpgrade(this);
        }
    }
}