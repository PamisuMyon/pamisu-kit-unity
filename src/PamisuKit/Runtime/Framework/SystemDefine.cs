
using UnityEngine;

namespace PamisuKit.Framework
{
    public interface ISystem
    {
        // void OnCreate();

        void OnDestroy();

        void Destroy();
    }

    public abstract class MonoSystem : MonoEntity, ISystem
    {
        // public virtual void OnCreate() {}
        
        public abstract void Destroy();
    }
    
    public abstract class MonoSystem<T> : MonoSystem where T : MonoSystem
    {
        public static T Instance { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            if (Instance != null)
            {
                Debug.LogWarning($"{GetType().Name} OnCreate Instance already exists.");
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
        }

        public override void Destroy()
        {
            if (Instance == null)
                Debug.LogWarning($"{GetType().Name} Destroy Instance is already null.");
            Destroy(gameObject);
            Instance = null;
        }
        
    }

}