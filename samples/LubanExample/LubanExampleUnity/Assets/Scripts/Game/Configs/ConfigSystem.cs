using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Luban;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    public class ConfigSystem : MonoSystem
    {
        private const string TableAssetsLabel = "Table";

        private Dictionary<string, byte[]> _tableBytesDict;
        private readonly ByteBuf _byteBuf = new ByteBuf();
        public Tables Tables { get; private set; }

        public UniTask Init()
        {
            return LoadAllTables();
        }

        private async UniTask LoadAllTables()
        {
            _tableBytesDict = new Dictionary<string, byte[]>();
            var handle = Addressables.LoadAssetsAsync<TextAsset>(TableAssetsLabel, _ => { });
            var textAssets = await handle.ToUniTask();
            foreach (var textAsset in textAssets)
            {
                _tableBytesDict.Add(textAsset.name, textAsset.bytes);
            }
            Addressables.Release(handle);
            Tables = new Tables(LoadByteBuf);
            Debug.Log($"{GetType().Name} All data tables loaded.");
        }
        
        private ByteBuf LoadByteBuf(string name)
        {
            if (!_tableBytesDict.TryGetValue(name, out var bytes))
            {
                Debug.LogError($"{GetType().Name} LoadByteBuf error: {name}");
                return null;
            }
        
            _byteBuf.Replace(bytes);
            return _byteBuf;
        }
    }
}