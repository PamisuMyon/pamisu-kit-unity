using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PamisuKit.Common;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class MonoEntity : MonoBehaviour, IEntity
    {
        [SerializeField]
        protected bool AutoSetup;
        
        public Transform Trans { get; private set; }

        public GameObject Go { get; private set; }
        
        public Region Region { get; private set; }
        
        public virtual bool IsActive => !IsPendingDestroy && Go.activeInHierarchy;

        public bool IsPendingDestroy { get; private set; }
        
        protected List<IDisposable> EventSubscriptions;

        protected virtual void Start()
        {
            if (AutoSetup && Region == null)
            {
                DoAutoSetup().Forget();
            }
        }

        protected virtual async UniTaskVoid DoAutoSetup()
        {
            await UniTask.Yield();
            if (Region != null)
                return;
            var director = FindFirstObjectByType<Director>();
            if (director != null)
            {
                director.SetupMonoEntity(this);
                OnAutoSetup();
            }
        }

        public void Setup(Region region)
        {
            if (Region != null)
                return;
            Go = gameObject;
            Trans = Go.transform;
            Region = region;
            Region.AddEntity(this);
            OnCreate();
        }

        protected virtual void OnCreate() 
        {
        }

        protected virtual void OnAutoSetup()
        {
        }

        public void OnDestroy()
        {
            IsPendingDestroy = true;
            ClearEventSubscriptions();
            if (Region != null)
            {
                OnSelfDestroy();
                Region.RemoveEntity(this);
            }
            Go = null;
            Trans = null;
            Region = null;
        }

        protected virtual void OnSelfDestroy()
        {
        }
        
        protected T FindChildComponent<T>(string path) where T : Component
        {
            var child = Trans.Find(path);
            Debug.Assert(child != null, $"{GetType().Name} can't find child, please check the path：{path} ", Trans);
            return child != null ? child.GetComponent<T>() : default;
        }
        
        public void On<TMessage>(Action<TMessage> handler, EventBus.SubscribeOptions option = EventBus.SubscribeOptions.None)
            where TMessage : struct
        {
            var disposable = EventBus.On(handler, option);
            EventSubscriptions ??= new List<IDisposable>();
            EventSubscriptions.Add(disposable);
        }

        public void Off<TMessage>(Action<TMessage> handler) where TMessage : struct => EventBus.Off(handler);

        public void Emit<TMessage>(TMessage message) where TMessage : struct => EventBus.Emit(message);

        protected void ClearEventSubscriptions()
        {
            if (EventSubscriptions == null)
                return;
            for (var i = 0; i < EventSubscriptions.Count; i++)
            {
                EventSubscriptions[i].Dispose();
            }
            EventSubscriptions.Clear();
        }
        
        public TDirector GetDirector<TDirector>() where TDirector : Director
        {
            return Region.GetDirector<TDirector>();
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return Region.GetSystem<TSystem>();
        }
        
        public ISystem GetSystem(Type type)
        {
            return Region.GetSystem(type);
        }

    }
}