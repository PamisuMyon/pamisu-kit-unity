using System;
using System.Collections.Generic;
using PamisuKit.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PamisuKit.Framework
{
    public class Entity : IEntity
    {

        public Transform Trans { get; protected set; }
        
        public GameObject Go { get; protected set; }
        
        public Region Region { get; protected set; }
        
        public virtual bool IsActive => !IsPendingDestroy && Go.activeInHierarchy;

        public bool IsPendingDestroy { get; private set; }

        protected List<IDisposable> EventSubscriptions; 

        public Entity()
        {
        }
        
        public Entity(GameObject go, Region region)
        {
            Go = go;
            Trans = go.transform;
            Region = region;
            Region.AddEntity(this);
        }
        
        public virtual void Destroy()
        {
            IsPendingDestroy = true;
            Region.RemoveEntity(this);
            ClearEventSubscriptions();
            OnDestroy();
            if (Go != null)
                Object.Destroy(Go);
            Trans = null;
            Go = null;
            Region = null;
        }
        
        public virtual void OnDestroy() {}
        
        protected T FindChildComponent<T>(string path) where T : Component
        {
            var child = Trans.Find(path);
            Debug.Assert(child != null, $"{GetType().Name} can't find child, please check the path：{path} ", Trans);
            return child != null ? child.GetComponent<T>() : default;
        }

        public int GetInstanceID()
        {
            if (Go != null)
                return Go.GetInstanceID();
            return 0;
        } 

        public void On<TMessage>(Action<TMessage> handler, EventBus.SubscribeOptions option = EventBus.SubscribeOptions.None)
            where TMessage : struct
        {
            var disposable = EventBus.On(handler, option);
            EventSubscriptions ??= new List<IDisposable>();
            EventSubscriptions.Add(disposable);
        }
        
        public void Off<TMessage>(Action<TMessage> handler) where TMessage : struct
        {
            EventBus.Off(handler);
        }

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
        
    }
}