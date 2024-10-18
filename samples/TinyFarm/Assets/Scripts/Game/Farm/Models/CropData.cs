using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Farm.Models
{
    public class CropData : ISerializee
    {
        public string SeedId;
        public float GrowthTimeCounter;
        public int PhaseIndex;
        public int RegrowthTimes;

        public SeedConfig Config { get; private set; }
        public bool IsRipe => PhaseIndex >= Config.Phases.Length - 1;

        public CropData(SeedConfig config, int phaseIndex = 0)
        {
            Config = config;
            SeedId = config.Id;
            PhaseIndex = phaseIndex;
        }

        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Config = App.Instance.GetSystem<ConfigSystem>().GetItemConfig(SeedId) as SeedConfig;
            if (Config == null)
                Debug.LogError($"SeedConfig of Id {SeedId} not found");
        }
    }
}