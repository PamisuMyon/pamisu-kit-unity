using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public abstract class BaseApp : MonoBehaviour
    {
        public Dictionary<Type, ISystem> SystemDict { get; protected set; }

        protected virtual void Awake()
        {
            OnCreate();
        }

        protected virtual void OnCreate()
        {
            SystemDict = new Dictionary<Type, ISystem>();
        }
        
        public virtual TSystem CreateMonoSystem<TSystem>(Transform parent = null) where TSystem : MonoSystem
        {
            var type = typeof(TSystem);
            if (SystemDict.ContainsKey(type))
            {
                Debug.LogWarning($"{GetType().Name} The system({type}) you want to create already exists.");
                return null;
            }
            
            if (parent == null)
                parent = transform;
            var system = parent.GetComponentInChildren<TSystem>();
            if (system == null)
            {
                var go = new GameObject(typeof(TSystem).Name);
                go.transform.SetParent(parent);
                system = go.AddComponent<TSystem>();
            }
            SystemDict.Add(type, system);
            return system;
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            if (SystemDict.TryGetValue(typeof(TSystem), out var system))
                return system as TSystem;
            return null;
        }

        public ISystem GetSystem(Type type)
        {
            if (SystemDict.TryGetValue(type, out var system))
                return system;
            return default;
        }

        public virtual void DestroySystem(ISystem system)
        {
            SystemDict.Remove(system.GetType());
            if (system is MonoSystem monoSystem)
                Destroy(monoSystem.gameObject);
        }

        protected void OnDestroy()
        {
            if (SystemDict == null)
                return;
            foreach (var it in SystemDict.Values)
            {
                if (it is MonoSystem monoSystem)
                    Destroy(monoSystem.gameObject);
            }
            SystemDict.Clear();
        }
        
    }
    
    public abstract class BaseApp<T> : BaseApp where T : BaseApp
    {

        public static T Instance { get; private set; }

        [SerializeField]
        private bool _dontDestroyOnLoad = false;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<T>();
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            base.Awake();
        }
        
    }
    
}