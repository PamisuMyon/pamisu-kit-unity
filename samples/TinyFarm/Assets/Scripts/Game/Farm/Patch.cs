using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Farm.Models;
using Game.Framework;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace Game.Farm
{
    public class Patch : Unit
    {

        private BoxCollider2D _collider;
        private Dictionary<string, Plot> _plotDict;
        
        public PatchData Data { get; private set; }

        public async UniTaskVoid Init(PatchData data)
        {
            Data = data;
            _plotDict = new Dictionary<string, Plot>();
            var configSystem = GetSystem<ConfigSystem>();
            var patchSystem = GetSystem<PatchSystem>();
            
            for (int i = 0, x = data.Min.x; x <= data.Max.x; x++)
            {
                for (int y = data.Min.y; y <= data.Max.y; y++)
                {
                    var cellPos = new Vector3Int(x, y);
                    patchSystem.SetPatchTile(cellPos);

                    if (x == data.Min.x 
                        || x == data.Max.x 
                        || y == data.Min.y 
                        || y == data.Max.y)
                        continue;
                    
                    if (i >= data.Plots.Count)
                        data.Plots.Add(new PlotData());
                    
                    var plotGo = await AssetManager.Instantiate(configSystem.PlotPrefabRef);
                    var plot = plotGo.GetComponent<Plot>();
                    plot.Setup(Region);
                    var plotPos = patchSystem.Tilemap.CellToWorld(cellPos);
                    plotPos += patchSystem.Tilemap.cellSize / 2f;
                    plot.Trans.position = plotPos;
                    plot.Init(data.Plots[i]);
                    _plotDict[plot.Id] = plot;
                    i++;
                }
            }
            
            VisualSize = data.Max - data.Min;
            var minPos = patchSystem.Tilemap.CellToWorld((Vector3Int)data.Min);
            var maxPos = patchSystem.Tilemap.CellToWorld((Vector3Int)data.Max);
            var centerPos = new Vector3((maxPos.x - minPos.x) / 2f, (maxPos.y - minPos.y) / 2f, 0f);
            Trans.position = centerPos;

            // _collider = Go.AddComponent<BoxCollider2D>();
            // _collider.size = VisualSize;
            // Go.layer = LayerMask.NameToLayer("Unit");
        }

        public Plot GetPlotById(string id)
        {
            return _plotDict.GetValueOrDefault(id);
        }
    }
    
    [Serializable]
    public class PatchInitInfo
    {
        public Vector2Int Min;
        public Vector2Int Max;
    }
}