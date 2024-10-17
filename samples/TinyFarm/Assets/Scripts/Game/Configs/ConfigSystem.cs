using System.Collections.Generic;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    public class ConfigSystem : MonoSystem
    {
        [SerializeField]
        private ItemConfig[] _itemConfigs;
        
        [Space]
        public AssetReferenceGameObject PlotPrefabRef;
        public AssetReferenceGameObject ProducePrefabRef;
        
        private readonly Dictionary<string, ItemConfig> _allItemConfigDict = new();

        protected override void OnCreate()
        {
            base.OnCreate();
            
            for (int i = 0; i < _itemConfigs.Length; i++)
            {
                if (!_allItemConfigDict.TryAdd(_itemConfigs[i].Id, _itemConfigs[i]))
                    Debug.LogError($"Duplicated Item Id: {_itemConfigs[i].Id} {_itemConfigs[i]}");
            }
        }
        
        public ItemConfig GetItemConfig(string id)
        {
            return _allItemConfigDict.GetValueOrDefault(id);
        }
        
    }
}