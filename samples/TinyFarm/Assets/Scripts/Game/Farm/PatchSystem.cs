using System.Collections.Generic;
using Game.Farm.Models;
using Game.Save;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Farm
{
    public class PatchSystem : MonoSystem, ISavable
    {
        
        public Tilemap Tilemap;
        public TileBase RuleTile;

        [SerializeField]
        private PatchInitInfo[] _initPatchInfos;

        private SaveSystem _saveSystem;
        private List<Patch> _patches;

        protected override void OnCreate()
        {
            base.OnCreate();
            _saveSystem = GetSystem<SaveSystem>();

            _patches = new List<Patch>();
            for (int i = 0; i < _initPatchInfos.Length; i++)
            {
                var patch = Region.NewMonoEntity<Patch>();
                patch.Init(_initPatchInfos[i]).Forget();
            }
            
            _saveSystem.RegisterSavable(this);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            if (_saveSystem != null)
                _saveSystem.RemoveSavable(this);
        }

        public void SetPatchTile(Vector3Int pos)
        {
            Tilemap.SetTile(pos, RuleTile);
        }

        public void OnSave(SaveData saveData)
        {
            for (int i = 0; i < _patches.Count; i++)
            {
                _patches[i].OnSave(saveData);
            }
        }

        public PatchData GetPatchData(int index)
        {
            return null;// TODO
        }
    }
    
}