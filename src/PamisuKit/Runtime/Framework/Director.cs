using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class Director : MonoBehaviour
    {
        protected List<ISystem> Systems;
        public Ticker Ticker { get; protected set; }
        public Region Region { get; protected set; }

        protected virtual void Awake()
        {
            OnCreate();
        }

        protected virtual void OnCreate()
        {
            Ticker = new Ticker();
            Region = new Region(Ticker, this);
            Systems = new List<ISystem>();
        }

        protected virtual void Update()
        {
            Ticker.OnUpdate(Time.deltaTime);
        }
        
        protected virtual void FixedUpdate()
        {
            Ticker.OnFixedUpdate(Time.fixedDeltaTime);
        }

        protected virtual void CreateMonoSystem<TSystem>() where TSystem : MonoSystem
        {
            var system = GetComponentInChildren<TSystem>();
            if (system == null)
            {
                var go = new GameObject(typeof(TSystem).Name);
                go.transform.SetParent(transform);
                system = go.AddComponent<TSystem>();
            }
            system.Setup(Region);
            Systems.Add(system);
            Ticker.Add(system);
        }

        protected virtual void DestroySystem(ISystem system)
        {
            Ticker.Remove(system);
            Systems.Remove(system);
            system.Destroy();
        }

        protected void OnDestroy()
        {
            if (Systems == null)
                return;
            for (var i = 0; i < Systems.Count; i++)
            {
                DestroySystem(Systems[i]);
            }
        }
    }

}