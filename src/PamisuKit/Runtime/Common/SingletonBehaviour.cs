using UnityEngine;

namespace PamisuKit.Common
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        [SerializeField]
        private bool _dontDestroyOnLoad = false;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<T>();
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

    }
}