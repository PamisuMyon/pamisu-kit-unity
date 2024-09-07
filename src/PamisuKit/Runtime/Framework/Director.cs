using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class Director : MonoBehaviour
    {

        public DirectorMode Mode = DirectorMode.Normal;
        
        protected List<ISystem> Systems;
        
        public BaseApp App { get; private set; }
        public Ticker Ticker { get; private set; }
        public Region Region { get; private set; }

        internal void Setup(BaseApp app)
        {
            if (App != null)
                return;
            App = app;
            OnCreate();
        }

        protected virtual void OnCreate()
        {
            Ticker = gameObject.AddComponent<Ticker>();
            Region = gameObject.AddComponent<Region>();
            Region.Init(Ticker, this);
            Systems = new List<ISystem>();
        }

        public virtual void SetupMonoEntity(MonoEntity entity)
        {
            entity.Setup(Region);
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
            if (Systems != null)
            {
                for (int i = 0; i < Systems.Count; i++)
                {
                    App.DestroySystem(Systems[i]);
                }
            }
        }

    }

    public enum DirectorMode
    {
        Normal, Global
    }

}