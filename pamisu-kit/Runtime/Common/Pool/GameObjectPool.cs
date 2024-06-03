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

        public static async UniTask<GameObjectPool> Create(object key, Transform root, int maxCapacity = -1)
        {
            object realKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            var prefab = await AssetManager.LoadAsset<GameObject>(realKey);
            var pool = new GameObjectPool(realKey, prefab, root, maxCapacity);
            return pool;
        }
        
        protected GameObjectPool(object key, GameObject prefab, Transform root, int maxCapacity = -1)
        {
            Key = key;
            Prefab = prefab;
            Root = root;
            MaxCapacity = maxCapacity;
            CreateInstanceFuncSync = CreateInstanceSync;
            AutoManagePoolElements = false;
        }

        protected override GameObject CreateInstanceSync()
        {
            return Object.Instantiate(Prefab, Root);
        }
        
        public virtual void Destroy()
        {
            AssetManager.Release(Key);
        }
        
    }
}