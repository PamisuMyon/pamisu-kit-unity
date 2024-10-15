using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Framework;
using Game.Save;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace Game.Farm
{
    public class Patch : Unit, ISavable
    {

        private BoxCollider2D _collider;
        private readonly List<Plot> _plots = new();

        public async UniTaskVoid Init(PatchInitInfo initInfo)
        {
            var configSystem = GetSystem<ConfigSystem>();
            var patchSystem = GetSystem<PatchSystem>();
            
            for (int x = initInfo.Min.x; x <= initInfo.Max.x; x++)
            {
                for (int y = initInfo.Min.y; y <= initInfo.Max.y; y++)
                {
                    var cellPos = new Vector3Int(x, y);
                    patchSystem.SetPatchTile(cellPos);

                    if (x == initInfo.Min.x || x == initInfo.Max.x
                        || y == initInfo.Min.y || y == initInfo.Max.y)
                        continue;
                    var plotGo = await AssetManager.Instantiate(configSystem.PlotPrefabRef);
                    var plot = plotGo.GetComponent<Plot>();
                    plot.Setup(Region);
                    var plotPos = patchSystem.Tilemap.CellToWorld(cellPos);
                    plotPos += patchSystem.Tilemap.cellSize / 2f;
                    plot.Trans.position = plotPos;
                    _plots.Add(plot);
                    // patchSystem.RegisterPlot(plot);
                }
            }
            
            VisualSize = initInfo.Max - initInfo.Min;
            var minPos = patchSystem.Tilemap.CellToWorld((Vector3Int)initInfo.Min);
            var maxPos = patchSystem.Tilemap.CellToWorld((Vector3Int)initInfo.Max);
            var centerPos = new Vector3((maxPos.x - minPos.x) / 2f, (maxPos.y - minPos.y) / 2f, 0f);
            Trans.position = centerPos;

            // _collider = Go.AddComponent<BoxCollider2D>();
            // _collider.size = VisualSize;
            // Go.layer = LayerMask.NameToLayer("Unit");
        }

        public void OnSave(SaveData saveData)
        {
            
        }
    }
    
    [Serializable]
    public class PatchInitInfo
    {
        public Vector2Int Min;
        public Vector2Int Max;
    }
}