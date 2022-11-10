using UnityEngine;

namespace Pamisu.Commons
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        [SerializeField]
        private bool _dontDestroyOnLoad = true;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<T>();
            if (_dontDestroyOnLoad)
                gameObject.DontDestroyOnLoad();
        }

    }
}
