using System.Collections.Generic;
using Game.Farm.Models;
using Game.Save;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Farm
{
    public class PatchSystem : MonoSystem
    {
        
        public Tilemap Tilemap;
        public TileBase RuleTile;

        [SerializeField]
        private PatchInitInfo[] _initPatchInfos;

        private SaveSystem _saveSystem;
        private List<Patch> _patches;

        public List<PatchData> PatchDataList => _saveSystem.SaveData.Farm.Patches;

        protected override void OnCreate()
        {
            base.OnCreate();
            _saveSystem = GetSystem<SaveSystem>();

            var patchDataList = PatchDataList;
            if (patchDataList.Count == 0)
            {
                for (int i = 0; i < _initPatchInfos.Length; i++)
                {
                    var patchData = new PatchData
                    {
                        Min = _initPatchInfos[i].Min,
                        Max = _initPatchInfos[i].Max,
                    };
                    patchData.PostDeserialize();
                    patchDataList.Add(patchData);
                }
            }
            _patches = new List<Patch>();
            for (int i = 0; i < patchDataList.Count; i++)
            {
                var patch = Region.NewMonoEntity<Patch>();
                patch.Init(patchDataList[i]).Forget();
            }
        }

        public void SetPatchTile(Vector3Int pos)
        {
            Tilemap.SetTile(pos, RuleTile);
        }
        
        public Plot GetPlotById(string id)
        {
            for (int i = 0; i < _patches.Count; i++)
            {
                var plot = _patches[i].GetPlotById(id);
                if (plot != null)
                    return plot;
            }
            return null;
        }

    }
    
}