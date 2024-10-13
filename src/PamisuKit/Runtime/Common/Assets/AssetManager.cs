using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PamisuKit.Common.Assets
{
    public enum AssetRefCountMode
    {
        Single,
        Multiple
    }

    public class AssetManager
    {
        private static readonly Dictionary<object, object> _assets = new();

        public static UniTask<T> LoadAsset<T>(object key, AssetRefCountMode mode = AssetRefCountMode.Single, CancellationToken cancellationToken = default)
        {
            if (mode == AssetRefCountMode.Single)
                return LoadAssetSingleRefCount<T>(key, cancellationToken);
            return LoadAssetMultipleRefCount<T>(key, cancellationToken);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static async UniTask<T> LoadAssetSingleRefCount<T>(object key, CancellationToken cancellationToken)
        {
            object dictKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            if (_assets.TryGetValue(dictKey, out var obj))
            {
                if (obj is T assetT)
                    return assetT;

                Debug.LogError($"AssetManager LoadAsset expected instance of {typeof(T)}, but got {obj.GetType()}");
                return default;
            }

            var asset = await Addressables.LoadAssetAsync<T>(key).ToUniTask(null, PlayerLoopTiming.Update,cancellationToken);
            if (_assets.TryGetValue(dictKey, out var obj1) && obj1 is T assetT1)
            {
                // Addressables.Release(key);
                return assetT1;
            }

            _assets[dictKey] = asset;
            return asset;
        }

        private static async UniTask<T> LoadAssetMultipleRefCount<T>(object key, CancellationToken cancellationToken)
        {
            var asset = await Addressables.LoadAssetAsync<T>(key).ToUniTask(null, PlayerLoopTiming.Update,cancellationToken);
            return asset;
        }

        public static async UniTask<GameObject> Instantiate(object key, Transform parent = null, bool inWorldSpace = false, bool trackHandle = true, CancellationToken cancellationToken = default)
        {
            var go = await Addressables.InstantiateAsync(key, parent, inWorldSpace, trackHandle).ToUniTask(null, PlayerLoopTiming.Update, cancellationToken);
            return go;
        }

        public static void Release(object key)
        {
            object dictKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            if (_assets.ContainsKey(dictKey))
            {
                _assets.Remove(dictKey);
            }
            Addressables.Release(dictKey);
        }

        public static void ReleaseInstance(GameObject go)
        {
            Addressables.ReleaseInstance(go);
        }
        
    }
}