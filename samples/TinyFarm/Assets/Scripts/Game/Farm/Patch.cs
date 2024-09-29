using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace Game.Farm
{
    public class Patch : Unit
    {

        private BoxCollider2D _collider;
        private readonly List<Plot> _plots = new();

        public async UniTaskVoid Init(PatchInfo info)
        {
            var configSystem = GetSystem<ConfigSystem>();
            var patchSystem = GetSystem<PatchSystem>();
            
            for (int x = info.Min.x; x <= info.Max.x; x++)
            {
                for (int y = info.Min.y; y <= info.Max.y; y++)
                {
                    var cellPos = new Vector3Int(x, y);
                    patchSystem.SetPatchTile(cellPos);

                    if (x == info.Min.x || x == info.Max.x
                        || y == info.Min.y || y == info.Max.y)
                        continue;
                    var plotGo = await AssetManager.Instantiate(configSystem.PlotPrefabRef);
                    var plot = plotGo.GetComponent<Plot>();
                    plot.Setup(Region);
                    var plotPos = patchSystem.Tilemap.CellToWorld(cellPos);
                    plotPos += patchSystem.Tilemap.cellSize / 2f;
                    plot.Trans.position = plotPos;
                    _plots.Add(plot);
                    patchSystem.Plots.Add(plot);
                }
            }
            
            VisualSize = info.Max - info.Min;
            var minPos = patchSystem.Tilemap.CellToWorld((Vector3Int)info.Min);
            var maxPos = patchSystem.Tilemap.CellToWorld((Vector3Int)info.Max);
            var centerPos = new Vector3((maxPos.x - minPos.x) / 2f, (maxPos.y - minPos.y) / 2f, 0f);
            Trans.position = centerPos;

            _collider = Go.AddComponent<BoxCollider2D>();
            _collider.size = VisualSize;
        }
        
    }
    
    [Serializable]
    public class PatchInfo
    {
        public Vector2Int Min;
        public Vector2Int Max;
    }
}