using Game.Framework;

namespace Game.Configs
{
    public abstract class GearUpgradeConfig : UpgradeConfig
    {
        public CharacterConfig Target;
        
        public abstract Upgrade CreateUpgrade();
    }
}