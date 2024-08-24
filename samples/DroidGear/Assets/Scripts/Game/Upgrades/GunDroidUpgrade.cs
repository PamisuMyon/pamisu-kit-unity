using Game.Configs;
using Game.Framework;

namespace Game.Modifiers
{
    public class GunDroidUpgrade : Upgrade
    {
        private GunDroidUpgradeConfig _config;
        
        public GunDroidUpgrade(UpgradeConfig config) : base(config)
        {
            _config = config as GunDroidUpgradeConfig;
        }
        
    }
}