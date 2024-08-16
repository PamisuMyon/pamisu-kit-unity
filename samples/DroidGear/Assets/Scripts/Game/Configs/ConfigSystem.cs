using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PamisuKit.Framework;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    public class ConfigSystem : MonoSystem
    {
        public Dictionary<string, CharacterConfig> Characters { get; private set;} = new();

        public async UniTask Init()
        {
            var characterLabels = new List<string> { "Character", "Config" };
            var characterConfigs = await Addressables.LoadAssetsAsync<CharacterConfig>(characterLabels, null, Addressables.MergeMode.Intersection).ToUniTask();
            for (int i = 0; i < characterConfigs.Count; i++)
            {
                characterConfigs[i].Init();
                Characters.Add(characterConfigs[i].Id, characterConfigs[i]);
            }
        }

    }
}