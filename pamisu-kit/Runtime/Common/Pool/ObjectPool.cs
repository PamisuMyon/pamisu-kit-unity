using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PamisuKit.Common.Pool
{
    public class ObjectPool<T>
    {

        protected Func<T> CreateInstanceFuncSync;
        protected Func<UniTask<T>> CreateInstanceFuncAsync;
        protected Action<T> DestroyInstanceFunc;

        protected int MaxCapacity;
        protected readonly Queue<T> AvailableInstances = new();
        protected readonly HashSet<T> InUseInstances = new();
        protected int Capacity => AvailableInstances.Count + InUseInstances.Count;
        protected bool AutoManagePoolElements;

        public ObjectPool(Func<T> createInstanceFuncSync = null, Action<T> destroyInstanceFunc = null, int maxCapacity = -1, bool autoManagePoolElement = true)
        {
            CreateInstanceFuncSync = createInstanceFuncSync?? CreateInstanceSync;
            DestroyInstanceFunc = destroyInstanceFunc;
            MaxCapacity = maxCapacity;
            AutoManagePoolElements = autoManagePoolElement;
        }
        
        public ObjectPool(Func<UniTask<T>> createInstanceFuncAsync = null, Action<T> destroyInstanceFunc = null, int maxCapacity = -1, bool autoManagePoolElement = true)
        {
            CreateInstanceFuncAsync = createInstanceFuncAsync?? CreateInstanceAsync;
            DestroyInstanceFunc = destroyInstanceFunc;
            MaxCapacity = maxCapacity;
            AutoManagePoolElements = autoManagePoolElement;
        }

        public ObjectPool()
        {
            MaxCapacity = -1;
        }

        protected virtual T CreateInstanceSync()
        {
            return default;
        }

        protected virtual UniTask<T> CreateInstanceAsync()
        {
            return UniTask.FromResult<T>(default);
        }

        public T SpawnSync()
        {
            if (AvailableInstances.Count == 0)
            {
                if (MaxCapacity != -1 && Capacity >= MaxCapacity)
                    return default;
                var newGo = CreateInstanceFuncSync();
                AvailableInstances.Enqueue(newGo);
            }
            var item = AvailableInstances.Dequeue();
            InUseInstances.Add(item);
            if (AutoManagePoolElements && item is IPoolElement poolElement)
                poolElement.OnSpawnFromPool();
            return item;
        }
        
        public async UniTask<T> SpawnAsync()
        {
            if (AvailableInstances.Count == 0)
            {
                if (MaxCapacity != -1 && Capacity >= MaxCapacity)
                    return default;
                var newItem = await CreateInstanceFuncAsync();
                AvailableInstances.Enqueue(newItem);
            }
            var item = AvailableInstances.Dequeue();
            InUseInstances.Add(item);
            if (AutoManagePoolElements && item is IPoolElement poolElement)
                poolElement.OnSpawnFromPool();
            return item;
        }
        
        public bool Release(T item)
        {
            if (AutoManagePoolElements && item is IPoolElement poolElement)
                poolElement.OnReleaseToPool();
            if (!InUseInstances.Remove(item))
            {
                var b = AvailableInstances.Contains(item);
                if (b)
                    Debug.LogError($"{GetType().Name} {item} has been released, please avoid duplicate releasing.");
                else
                    Debug.LogError($"{GetType().Name} can't release {item} because it's not belong to this pool.");
                return b;
            }
            if (MaxCapacity != -1 && Capacity >= MaxCapacity)
            {
                DestroyInstanceFunc?.Invoke(item);
                return true;
            }
            AvailableInstances.Enqueue(item);
            return true;
        }

    }
}