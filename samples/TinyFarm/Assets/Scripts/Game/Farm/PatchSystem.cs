using System.Collections.Generic;
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
        private PatchInfo[] _initPatchInfos;
        
        public List<Plot> Plots { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Plots = new List<Plot>();
            for (int i = 0; i < _initPatchInfos.Length; i++)
            {
                var patch = Region.NewMonoEntity<Patch>();
                patch.Init(_initPatchInfos[i]).Forget();
            }
        }

        public void SetPatchTile(Vector3Int pos)
        {
            Tilemap.SetTile(pos, RuleTile);
        }

        public void RegisterPlot(Plot plot)
        {
            Plots.Add(plot);
        }
        
    }
    
}