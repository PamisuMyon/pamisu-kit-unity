using PamisuKit.Common.Assets;
using PamisuKit.Common.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PamisuKit.Framework
{
    
    public delegate S CreateEntityDelegate<out S>(GameObject go, Region region) where S : Entity;

    public class EntityPool<T> : ObjectPool<T> where T : Entity
    {
        protected string _address;
        protected GameObject _prefab;
        protected Region _region;
        protected CreateEntityDelegate<T> _createEntityFunc;

        public static async UniTask<EntityPool<R>> Create<R>(string address, Region region, CreateEntityDelegate<R> createEntityDelegate, int maxCapacity = -1) where R : Entity
        {
            var prefab = await AssetManager.LoadAsset<GameObject>(address);
            var pool = new EntityPool<R>(address, prefab, region, createEntityDelegate, maxCapacity);
            return pool;
        }
        
        protected EntityPool(string address, GameObject prefab, Region region, CreateEntityDelegate<T> createEntityDelegate, int maxCapacity = -1)
        {
            _address = address;
            _prefab = prefab;
            _region = region;
            MaxCapacity = maxCapacity;
            _createEntityFunc = createEntityDelegate;
            CreateInstanceFuncSync = CreateInstanceSync;
        }

        protected override T CreateInstanceSync()
        {
            var go = Object.Instantiate(_prefab, _region.Trans);
            return _createEntityFunc(go, _region);
        }
        
        public virtual void Destroy()
        {
            AssetManager.Release(_address);
        }
        
    }
}