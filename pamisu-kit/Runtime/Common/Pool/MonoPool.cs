using System;
using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace PamisuKit.Common.Pool
{
    public class MonoPool<T> : ObjectPool<T> where T : Component 
    {
        protected string Address;
        protected GameObject Prefab;
        protected Transform Root;

        public static async UniTask<MonoPool<T>> Create(string address, Transform root, int maxCapacity = -1, bool autoManagePoolElement = true)
        {
            var prefab = await AssetManager.LoadAsset<GameObject>(address);
            var pool = new MonoPool<T>(address, prefab, root, maxCapacity, autoManagePoolElement);
            return pool;
        }
        
        protected MonoPool(string address, GameObject prefab, Transform root, int maxCapacity = -1, bool autoManagePoolElement = true)
        {
            Address = address;
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
            AssetManager.Release(Address);
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