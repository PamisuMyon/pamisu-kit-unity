using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PamisuKit.Common.Pool
{
    public class MonoPool<T> : ObjectPool<T> where T : Component 
    {
        protected object Key;
        protected GameObject Prefab;
        protected Transform Root;

        public static async UniTask<MonoPool<T>> Create(object key, Transform root, int maxCapacity = -1, bool autoManagePoolElement = true, CancellationToken cancellationToken = default)
        {
            object realKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            var prefab = await AssetManager.LoadAsset<GameObject>(realKey, AssetRefCountMode.Single, cancellationToken);
            var pool = new MonoPool<T>(realKey, prefab, root, maxCapacity, autoManagePoolElement);
            return pool;
        }
        
        protected MonoPool(object key, GameObject prefab, Transform root, int maxCapacity = -1, bool autoManagePoolElement = true)
        {
            Key = key;
            Prefab = prefab;
            Root = root;
            MaxCapacity = maxCapacity;
            AutoManagePoolElements = autoManagePoolElement;
            CreateInstanceFuncSync = CreateInstanceSync;
        }

        protected override T CreateInstanceSync()
        {
            var go = UnityEngine.Object.Instantiate(Prefab, Root);
            if (!go.TryGetComponent(out T component))
                component = go.AddComponent<T>();
            return component;
        }
        
        public virtual void Destroy()
        {
            AssetManager.Release(Key);
        }
    }

/* 
    // Non-generic implementation
    public class MonoPool : ObjectPool<MonoBehaviour>
    {
        protected Type CompType;
        protected string Address;
        protected GameObject Prefab;
        protected Transform Root;

        public static UniTask<MonoPool> Create<T>(string address, Transform root, int maxCapacity = -1)
        {
            return Create(typeof(T), address, root, maxCapacity);
        }

        public static async UniTask<MonoPool> Create(Type type, string address, Transform root, int maxCapacity = -1)
        {
            var prefab = await AssetManager.LoadAsset<GameObject>(address);
            var pool = new MonoPool(type, address, prefab, root, maxCapacity);
            return pool;
        }
        
        protected MonoPool(Type type, string address, GameObject prefab, Transform root, int maxCapacity = -1)
        {
            CompType = type;
            Address = address;
            Prefab = prefab;
            Root = root;
            MaxCapacity = maxCapacity;
            CreateInstanceFuncSync = CreateInstanceSync;
        }

        protected override MonoBehaviour CreateInstanceSync()
        {
            var go = UnityEngine.Object.Instantiate(Prefab, Root);
            if (!go.TryGetComponent(CompType, out var component))
                component = go.AddComponent(CompType);
            return component as MonoBehaviour;
        }
        
        public virtual void Destroy()
        {
            AssetManager.Release(Address);
        }
    }

*/

}