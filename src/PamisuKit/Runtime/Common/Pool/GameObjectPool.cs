using System.Threading;
using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PamisuKit.Common.Pool
{
    public class GameObjectPool : ObjectPool<GameObject>
    {
        protected object Key;
        protected GameObject Prefab;
        protected Transform Root;
        
        public static GameObjectPool Create(GameObject prefab, Transform root, int maxCapacity = -1)
        {
            object key = prefab;
            var pool = new GameObjectPool(key, prefab, root, maxCapacity);
            return pool;
        }

        public static async UniTask<GameObjectPool> Create(object key, Transform root, int maxCapacity = -1, CancellationToken cancellationToken = default)
        {
            object realKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            var prefab = await AssetManager.LoadAsset<GameObject>(realKey, AssetRefCountMode.Single, cancellationToken);
            var pool = new GameObjectPool(realKey, prefab, root, maxCapacity);
            return pool;
        }
        
        protected GameObjectPool(object key, GameObject prefab, Transform root, int maxCapacity = -1)
        {
            Key = key;
            Prefab = prefab;
            Root = root;
            MaxCapacity = maxCapacity;
            CreateInstanceFunc = CreateInstance;
            AutoManagePoolElements = false;
        }

        protected override GameObject CreateInstance()
        {
            return Object.Instantiate(Prefab, Root);
        }
        
        public virtual void Destroy()
        {
            AssetManager.Release(Key);
        }
        
    }
}