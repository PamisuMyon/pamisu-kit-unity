using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class Director : MonoBehaviour, IDirector
    {

        public DirectorMode Mode = DirectorMode.Normal;
        
        protected List<ISystem> Systems;
        
        public Transform Trans { get; private set; }
        public GameObject Go { get; private set; }
        public AppDirector AppDirector { get; private set; }
        public Ticker Ticker { get; private set; }
        public Region Region { get; private set; }

        internal void Setup(AppDirector appDirector)
        {
            if (AppDirector != null)
                return;
            Go = gameObject;
            Trans = Go.transform;
            AppDirector = appDirector;
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
            var system = AppDirector.CreateMonoSystem<TSystem>(this);
            Systems.Add(system);
            return system;
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return AppDirector.GetSystem<TSystem>();
        }

        public ISystem GetSystem(Type type)
        {
            return AppDirector.GetSystem(type);
        }

        protected void DestroySystem(ISystem system)
        {
            AppDirector.DestroySystem(system);
            Systems.Remove(system);
        }
        
        public void RegisterService<TService>(TService service) where TService : class
        {
            AppDirector.RegisterService(service);
        }

        public TService GetService<TService>() where TService : class
        {
            return AppDirector.GetService<TService>();
        }

        public bool RemoveService<TService>()
        {
            return AppDirector.RemoveService<TService>();
        }

        public bool RemoveService(object service)
        {
            return AppDirector.RemoveService(service);
        }

        protected void OnDestroy()
        {
            if (Systems != null)
            {
                for (int i = 0; i < Systems.Count; i++)
                {
                    AppDirector.DestroySystem(Systems[i]);
                }
            }
        }

    }

}