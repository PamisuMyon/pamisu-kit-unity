using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs.Upgrades
{
    public abstract class UpgradeConfig : ScriptableObject
    {
        
        public AssetReferenceSprite Icon;
        public string Description;
        
        public abstract Upgrade CreateUpgrade();
        
    }
}
