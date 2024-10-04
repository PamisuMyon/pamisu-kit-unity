using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "Seed_", menuName = "Configs/SeedConfig", order = 0)]
    public class SeedConfig : ItemConfig
    {
        [Header("Seed")]
        public GrowthPhase[] Phases;
        public int RegrowthTimes;
    }

    [Serializable]
    public class GrowthPhase
    {
        public AssetReferenceSprite SpriteRef;
        public float Duration;
    }
}