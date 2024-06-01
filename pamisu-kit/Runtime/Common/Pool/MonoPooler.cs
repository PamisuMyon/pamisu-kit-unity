using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PamisuKit.Common.Pool
{
    public class MonoPooler
    {
        private readonly Dictionary<string, object> _poolDic = new();
        private readonly Dictionary<int, object> _instanceToPoolDic = new();
        private readonly Transform _root;

        public MonoPooler(Transform root)
        {
            if (root != null)
                _root = root;
            else
                _root = new GameObject("MonoPoolerRoot").transform;
        }
        
        public async UniTask<T> Spawn<T>(string address, int maxCapacity = -1) where T : Component
        {
            MonoPool<T> pool;
            if (_poolDic.TryGetValue(address, out var poolObj))
            {
                pool = poolObj as MonoPool<T>;
            }
            else
            {
                pool = await MonoPool<T>.Create(address, _root, maxCapacity);
                if (_poolDic.TryGetValue(address, out var value))
                    pool = value as MonoPool<T>;
                _poolDic[address] = pool;
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