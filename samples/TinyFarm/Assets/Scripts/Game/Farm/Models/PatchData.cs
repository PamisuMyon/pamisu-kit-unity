using System.Collections.Generic;
using Game.Framework;
using UnityEngine;

namespace Game.Farm.Models
{
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

            for (int i = 0; i < Plots.Count; i++)
            {
                Plots[i].PostDeserialize();
            }
        }
    }
}