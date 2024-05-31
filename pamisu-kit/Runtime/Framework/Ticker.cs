using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

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
    
    [Serializable]
    public class Ticker
    {
        public float TimeScale { get; set; } = 1f;
        public float DeltaTime { get; private set; }
        public float TickerTime { get; private set; }
        
        protected readonly List<IUpdatable> _updateObjects = new List<IUpdatable>();
        protected readonly List<IFixedUpdatable> _fixedUpdateObjects = new List<IFixedUpdatable>();

        public void Add(object obj)
        {
            if (obj is IUpdatable updatableObj)
                _updateObjects.Add(updatableObj);
            if (obj is IFixedUpdatable fixedUpdatableObj)
                _fixedUpdateObjects.Add(fixedUpdatableObj);
        }

        public void Remove(object obj)
        {
            if (obj is IUpdatable updatableObj)
                _updateObjects.Remove(updatableObj);
            if (obj is IFixedUpdatable fixedUpdatableObj)
                _fixedUpdateObjects.Remove(fixedUpdatableObj);
        }

        public void OnUpdate(float delta)
        {
            delta *= TimeScale;
            TickerTime += delta;
            DeltaTime = delta;
            if (_updateObjects.Count == 0) return;
            for (var i = 0; i < _updateObjects.Count; i++)
            {
                if (_updateObjects[i].IsActive)
                    _updateObjects[i].OnUpdate(delta);
            }
        }

        public void OnFixedUpdate(float delta)
        {
            if (TimeScale == 0)
                return;
            if (_fixedUpdateObjects.Count == 0) return;
            for (var i = 0; i < _fixedUpdateObjects.Count; i++)
            {
                if (_fixedUpdateObjects[i].IsActive)
                    _fixedUpdateObjects[i].OnFixedUpdate(delta);
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