using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Farm
{
    public class Patch : Unit
    {
        private List<Plot> _plots;

        public void Init(PatchInfo info)
        {
            var configSystem = GetSystem<ConfigSystem>();
            var patchSystem = GetSystem<PatchSystem>();
            for (int x = info.Min.x; x < info.Max.x; x++)
            {
                for (int y = info.Min.y; y < info.Max.y; y++)
                {
                    patchSystem.SetPatchTile(new Vector3Int(x, y));
                    // instantiate plot
                }
            }
            // size
        }
        
    }
    
    [Serializable]
    public class PatchInfo
    {
        public Vector2Int Min;
        public Vector2Int Max;
    }
}