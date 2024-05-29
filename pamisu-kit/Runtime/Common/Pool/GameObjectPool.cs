using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using UnityEngine;

namespace PamisuKit.Common.Pool
{
    public class GameObjectPool : ObjectPool<GameObject>
    {
        protected string Address;
        protected GameObject Prefab;
        protected Transform Root;

        public static async UniTask<GameObjectPool> Create(string address, Transform root, int maxCapacity = -1)
        {
            var prefab = await AssetManager.LoadAsset<GameObject>(address);
            var pool = new GameObjectPool(address, prefab, root, maxCapacity);
            return pool;
        }
        
        protected GameObjectPool(string address, GameObject prefab, Transform root, int maxCapacity = -1)
        {
            Address = address;
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
            AssetManager.Release(Address);
        }
        
    }
}