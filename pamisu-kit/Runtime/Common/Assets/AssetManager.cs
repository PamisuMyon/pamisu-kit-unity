using System;
using System.Collections.Generic;
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

        public static UniTask<T> LoadAsset<T>(object key, AssetRefCountMode mode = AssetRefCountMode.Single)
        {
            if (mode == AssetRefCountMode.Single)
                return LoadAssetSingleRefCount<T>(key);
            return LoadAssetMultipleRefCount<T>(key);
        }

        private static async UniTask<T> LoadAssetSingleRefCount<T>(object key)
        {
            object dictKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            if (_assets.TryGetValue(dictKey, out var obj))
            {
                if (obj is T assetT)
                    return assetT;

                Debug.LogError($"AssetManager LoadAsset expected instance of {typeof(T)}, but got {obj.GetType()}");
                return default;
            }

            try
            {
                var asset = await Addressables.LoadAssetAsync<T>(key).ToUniTask();
                if (_assets.TryGetValue(dictKey, out var obj1) && obj1 is T assetT)
                {
                    // Addressables.Release(key);
                    return assetT;
                }

                _assets[dictKey] = asset;
                return asset;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetManager LoadAsset error: {e.Message}");
                Debug.LogException(e);

                return default;
            }
        }

        private static async UniTask<T> LoadAssetMultipleRefCount<T>(object key)
        {
            try
            {
                var asset = await Addressables.LoadAssetAsync<T>(key).ToUniTask();
                return asset;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetManager LoadAsset error: {e.Message}");
                Debug.LogException(e);

                return default;
            }
        }

        public static async UniTask<GameObject> Instantiate(object key, Transform parent = null, bool inWorldSpace = false, bool trackHandle = true)
        {
            try
            {
                var go = await Addressables.InstantiateAsync(key, parent, inWorldSpace, trackHandle).ToUniTask();
                return go;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetManager Instantiate error: {e.Message}");
                Debug.LogException(e);

                return default;
            }
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