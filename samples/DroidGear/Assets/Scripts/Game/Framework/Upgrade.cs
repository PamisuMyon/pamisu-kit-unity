using Game.Configs.Upgrades;

namespace Game.Framework
{
    public abstract class Upgrade
    {

        public UpgradeConfig Config;
        public UpgradeComponent Comp { get; protected set; }
        public Character Owner => Comp.Owner;

        public Upgrade(UpgradeConfig config)
        {
            Config = config;
        }

        public virtual void OnApplied(UpgradeComponent comp)
        {
            Comp = comp;
        }

        public virtual void OnRemoved()
        {
            Comp = null;
        }
        
    }

}