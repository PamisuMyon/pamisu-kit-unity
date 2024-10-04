using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PamisuKit.Common.Pool
{
    public class MonoPooler
    {
        private readonly Dictionary<object, object> _poolDic = new();
        private readonly Dictionary<int, object> _instanceToPoolDic = new();
        private readonly Transform _root;

        public MonoPooler(Transform root)
        {
            if (root != null)
                _root = root;
            else
                _root = new GameObject("MonoPoolerRoot").transform;
        }

        public T Spawn<T>(GameObject prefab, int maxCapacity = -1) where T : Component
        {
            MonoPool<T> pool;
            if (_poolDic.TryGetValue(prefab, out var poolObj))
            {
                pool = poolObj as MonoPool<T>;
            }
            else
            {
                pool = MonoPool<T>.Create(prefab, _root, maxCapacity);
                if (_poolDic.TryGetValue(prefab, out var value))
                    pool = value as MonoPool<T>;
                _poolDic[prefab] = pool;
            }
            
            var item = pool.Spawn();
            if (item != null)
                _instanceToPoolDic.TryAdd(item.gameObject.GetInstanceID(), pool);
            return item;
        }
        
        public async UniTask<T> Spawn<T>(object key, int maxCapacity = -1, CancellationToken cancellationToken = default) where T : Component
        {
            object realKey = key is IKeyEvaluator? (key as IKeyEvaluator).RuntimeKey : key;
            MonoPool<T> pool;
            if (_poolDic.TryGetValue(realKey, out var poolObj))
            {
                pool = poolObj as MonoPool<T>;
            }
            else
            {
                pool = await MonoPool<T>.Create(realKey, _root, maxCapacity, true, cancellationToken);
                if (_poolDic.TryGetValue(realKey, out var value))
                    pool = value as MonoPool<T>;
                _poolDic[realKey] = pool;
            }
            
            var item = pool.Spawn();
            if (item != null)
                _instanceToPoolDic.TryAdd(item.gameObject.GetInstanceID(), pool);
            return item;
        }
        
        public void Release<T>(T item) where T : Component
        {
            var id = item.gameObject.GetInstanceID();
            if (_instanceToPoolDic.TryGetValue(id, out var poolObj))
            {
                var pool = poolObj as MonoPool<T>;
                pool.Release(item);
                _instanceToPoolDic.Remove(id);
            }
        }
        
    }

}