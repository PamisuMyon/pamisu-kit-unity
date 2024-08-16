using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PamisuKit.Framework
{
    public interface IUpdatable
    {
        bool IsActive { get; }
        
        void OnUpdate(float deltaTime);
    }
    
    public interface IFixedUpdatable
    {
        bool IsActive { get; }

        void OnFixedUpdate(float deltaTime);
    }
    
    public class Ticker : MonoBehaviour
    {
        public float TimeScale = 1f;
        public float DeltaTime { get; private set; }
        public float TickerTime { get; private set; }
        
        protected readonly List<IUpdatable> UpdateObjects = new List<IUpdatable>();
        protected readonly List<IFixedUpdatable> FixedUpdateObjects = new List<IFixedUpdatable>();

        public void Add(object obj)
        {
            if (obj is IUpdatable updatableObj)
                UpdateObjects.Add(updatableObj);
            if (obj is IFixedUpdatable fixedUpdatableObj)
                FixedUpdateObjects.Add(fixedUpdatableObj);
        }

        public void Remove(object obj)
        {
            if (obj is IUpdatable updatableObj)
                UpdateObjects.Remove(updatableObj);
            if (obj is IFixedUpdatable fixedUpdatableObj)
                FixedUpdateObjects.Remove(fixedUpdatableObj);
        }

        private void Update()
        {
            var delta = Time.deltaTime;
            delta *= TimeScale;
            TickerTime += delta;
            DeltaTime = delta;
            if (UpdateObjects.Count == 0) return;
            for (var i = 0; i < UpdateObjects.Count; i++)
            {
                if (UpdateObjects[i].IsActive)
                    UpdateObjects[i].OnUpdate(delta);
            }
        }

        private void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;
            if (FixedUpdateObjects.Count == 0) return;
            for (var i = 0; i < FixedUpdateObjects.Count; i++)
            {
                if (FixedUpdateObjects[i].IsActive)
                    FixedUpdateObjects[i].OnFixedUpdate(delta);
            }
        }

        public async UniTask Delay(float seconds, CancellationToken cancellationToken = default)
        {
            var time = seconds;
            while (time > 0)
            {
                await UniTask.Yield();
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                time -= DeltaTime;
            }
        }
        
    }
}