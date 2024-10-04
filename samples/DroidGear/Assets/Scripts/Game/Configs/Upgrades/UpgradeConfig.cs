using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Game.Configs.Upgrades
{
    public abstract class UpgradeConfig : ScriptableObject
    {
        
        public AssetReferenceSprite IconRef;
        public string Description;
        
        public abstract Upgrade CreateUpgrade();
        
    }
}
