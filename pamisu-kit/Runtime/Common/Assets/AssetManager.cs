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
        private static readonly Dictionary<string, object> _assets = new Dictionary<string, object>();

        public static UniTask<T> LoadAsset<T>(string key, AssetRefCountMode mode = AssetRefCountMode.Single)
        {
            if (mode == AssetRefCountMode.Single)
                return LoadAssetSingleRefCount<T>(key);
            return LoadAssetMultipleRefCount<T>(key);
        }

        private static async UniTask<T> LoadAssetSingleRefCount<T>(string key)
        {
            if (_assets.TryGetValue(key, out var obj))
            {
                if (obj is T assetT)
                    return assetT;

                Debug.LogError($"AssetManager LoadAsset expected instance of {typeof(T)}, but got {obj.GetType()}");
                return default;
            }

            try
            {
                var asset = await Addressables.LoadAssetAsync<T>(key).ToUniTask();
                if (_assets.TryGetValue(key, out var obj1) && obj1 is T assetT)
                {
                    // Addressables.Release(key); // 减少本次多余的引用计数
                    return assetT;
                }

                _assets[key] = asset;
                return asset;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetManager LoadAsset error: {e.Message}");
                Debug.LogException(e);

                return default;
            }
        }

        private static async UniTask<T> LoadAssetMultipleRefCount<T>(string key)
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

        public static async UniTask<GameObject> Instantiate(string key, Transform parent = null, bool inWorldSpace = false, bool trackHandle = true)
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

        public static void Release(string key)
        {
            if (_assets.ContainsKey(key))
            {
                _assets.Remove(key);
            }
            Addressables.Release(key);
        }

        public static void ReleaseInstance(GameObject go)
        {
            Addressables.ReleaseInstance(go);
        }
        
    }
}