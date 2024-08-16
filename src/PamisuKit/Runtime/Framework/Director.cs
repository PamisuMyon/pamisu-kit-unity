using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class Director : MonoBehaviour
    {

        protected List<ISystem> Systems;
        
        public BaseApp App { get; protected set; }
        public Ticker Ticker { get; protected set; }
        public Region Region { get; protected set; }

        protected virtual void Awake()
        {
            App = FindFirstObjectByType<BaseApp>();
            OnCreate();
        }

        protected virtual void OnCreate()
        {
            Ticker = gameObject.AddComponent<Ticker>();
            Region = gameObject.AddComponent<Region>();
            Region.Init(Ticker, this);
            Systems = new List<ISystem>();
        }

        protected virtual TSystem CreateMonoSystem<TSystem>() where TSystem : MonoSystem
        {
            var system = App.CreateMonoSystem<TSystem>(transform);
            system.Setup(Region);
            Systems.Add(system);
            return system;
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return App.GetSystem<TSystem>();
        }

        public ISystem GetSystem(Type type)
        {
            return App.GetSystem(type);
        }

        protected void DestroySystem(ISystem system)
        {
            App.DestroySystem(system);
            Systems.Remove(system);
        }

        protected void OnDestroy()
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                App.DestroySystem(Systems[i]);
            }
            Systems.Clear();
        }

    }

}