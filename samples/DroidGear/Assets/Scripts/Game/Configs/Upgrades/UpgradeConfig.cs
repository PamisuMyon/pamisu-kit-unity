using System;
using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{

    // [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/Upgrades/UpgradeConfig", order = 0)]
    public abstract class UpgradeConfig : ScriptableObject
    {
        
        [Header("Upgrade")]
        public AssetReferenceSprite Icon;
        public string Description;
        
        public abstract Upgrade CreateUpgrade();
    }
}
