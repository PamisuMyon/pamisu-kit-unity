using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Farm.Models
{
    public class FarmData : ISerializee
    {
        public List<PatchData> Patches;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Patches ??= new List<PatchData>();
            
            for (int i = 0; i < Patches.Count; i++)
            {
                Patches[i].PostDeserialize();
            }
        }
    }

    public class PatchData : ISerializee
    {
        public Vector2Int Min;
        public Vector2Int Max;
        public List<PlotData> Plots;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Plots ??= new List<PlotData>();
        }
    }

    public class PlotData
    {
        public string Id;
        public bool IsWatered;
        public CropData Crop;
        public bool HasCrop => Crop == null;
    }

    public class CropData
    {
        public string SeedId;
        [NonSerialized]
        public SeedConfig Config;
        public int PhaseIndex;
        public bool IsRipe;

        public CropData(SeedConfig config, int phaseIndex = 0)
        {
            Config = config;
            SeedId = config.Id;
            PhaseIndex = phaseIndex;
        }
    }
    
}