using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Farm
{
    public class CropPatchManager : MonoBehaviour
    {
        
        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private TileBase _cropRuleTile;

        [SerializeField]
        private CropPatchInfo[] _patchInfos;
        
        private void Awake()
        {
            for (int i = 0; i < _patchInfos.Length; i++)
            {
                InitCropPath(_patchInfos[i]);
            }
        }

        private void InitCropPath(CropPatchInfo info)
        {
            for (int x = info.Min.x; x < info.Max.x; x++)
            {
                for (int y = info.Min.y; y < info.Max.y; y++)
                {
                    _tilemap.SetTile(new Vector3Int(x, y), _cropRuleTile);
                }
            }
        }
    }
    
    [Serializable]
    public class CropPatchInfo
    {
        public Vector2Int Min;
        public Vector2Int Max;
    }
}