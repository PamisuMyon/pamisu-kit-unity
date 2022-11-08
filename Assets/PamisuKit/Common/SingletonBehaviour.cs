using UnityEngine;

namespace Pamisu.Common
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        [SerializeField]
        private bool dontDestroyOnLoad = true;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this.GetComponent<T>();
            if (dontDestroyOnLoad)
                gameObject.DontDestroyOnLoad();
        }

    }
}
