using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PamisuKit.Common.Pool
{
    public class GameObjectPooler
    {
        private readonly Dictionary<string, GameObjectPool> _poolDic = new();
        private readonly Dictionary<int, GameObjectPool> _instanceToPoolDic = new();
        private Transform _root;

        public GameObjectPooler(Transform root)
        {
            if (root != null)
                _root = root;
            else
                _root = new GameObject("GameObjectPoolerRoot").transform;
        }
        
        public async UniTask<GameObject> Spawn(string address, int maxCapacity = -1)
        {
            if (!_poolDic.TryGetValue(address, out var pool))
            {
                pool = await GameObjectPool.Create(address, _root, maxCapacity);
                if (_poolDic.TryGetValue(address, out var value))
                    pool = value;
                _poolDic[address] = pool;
            }
            var go = pool.SpawnSync();
            if (go != null)
                _instanceToPoolDic.TryAdd(go.GetInstanceID(), pool);
            return go;
        }
        
        public void Release(GameObject go)
        {
            var id = go.GetInstanceID();
            if (_instanceToPoolDic.TryGetValue(id, out var pool))
            {
                pool.Release(go);
                _instanceToPoolDic.Remove(id);
            }
        }
        
    }
    
    /*
    public class GameObjectPooler : Singleton<GameObjectPooler>
    {
        private readonly Dictionary<string, GameObjectPool> _poolDic = new();
        private readonly Dictionary<int, GameObjectPool> _instanceToPoolDic = new();
        private Transform _root;
        private bool _isInitialized;

        private void Init()
        {
            if (_isInitialized)
                return;
            _root = new GameObject("GameObjectPoolerRoot").transform;
            _isInitialized = true;
        }
        
        public static async UniTask<GameObject> Spawn(string address, int maxCapacity = -1)
        {
            if (!Instance._isInitialized)
                Instance.Init();

            if (!Instance._poolDic.TryGetValue(address, out var pool))
            {
                pool = await GameObjectPool.Create(address, Instance._root, maxCapacity);
                if (Instance._poolDic.TryGetValue(address, out var value))
                    pool = value;
                Instance._poolDic[address] = pool;
            }
            var go = pool.SpawnSync();
            if (go != null)
                Instance._instanceToPoolDic.TryAdd(go.GetInstanceID(), pool);
            return go;
        }
        
        public static void Release(GameObject go)
        {
            if (!Instance._isInitialized)
            {
                Debug.LogError($"GameObjectPooler is not initialized, releasing of {go.name} has failed");
                return;
            }

            var id = go.GetInstanceID();
            if (Instance._instanceToPoolDic.TryGetValue(id, out var pool))
            {
                pool.Release(go);
                Instance._instanceToPoolDic.Remove(id);
            }
        }
        
    }
    */
}