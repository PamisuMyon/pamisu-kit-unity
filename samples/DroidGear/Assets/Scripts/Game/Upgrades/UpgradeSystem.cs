using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine.AddressableAssets;

namespace Game.Modifiers
{
    public class UpgradeSystem : MonoSystem
    {

        private IList<UpgradeConfig> _configs;
        private Dictionary<CharacterConfig, List<UpgradeConfig>> _poolDict;

        public async UniTask Init()
        {
            var labels = new List<string> { "Preload", "UpgradeConfig" };
            _configs = await Addressables.LoadAssetsAsync<UpgradeConfig>(labels, null, Addressables.MergeMode.Intersection).ToUniTask();
        }
    }
    
}