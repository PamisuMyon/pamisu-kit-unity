using System.Collections.Generic;
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
        public bool IsWatered;
        public bool HasCrop;
        public CropData Crop;
    }

    public class CropData
    {
        public string SeedId;
        public int PhaseIndex;
    }
    
}