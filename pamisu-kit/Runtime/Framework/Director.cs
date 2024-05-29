using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class Director<T> : MonoBehaviour where T : MonoBehaviour
    {

        public static T Instance { get; private set; }

        [SerializeField]
        private bool _dontDestroyOnLoad = false;
        
        protected Ticker Ticker;
        protected Region Region;
        protected List<ISystem> Systems;

        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<T>();
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            OnCreate();
        }

        protected virtual void OnCreate()
        {
            Ticker = new Ticker();
            Region = new Region(Ticker, transform);
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

        protected virtual void CreateSystem<TSystem>() where TSystem : System, new()
        {
            var system = new TSystem();
            system.SetupEntity(null, Region);
            system.OnCreate();
            Systems.Add(system);
        }

        protected virtual void CreateMonoSystem<TSystem>() where TSystem : MonoSystem
        {
            var system = GetComponentInChildren<TSystem>();
            if (system == null)
            {
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(transform);
                system = go.AddComponent<TSystem>();
                system.SetupEntity(Region);
            }
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