using UnityEngine;

namespace Pamisu.Common
{
    public abstract class SingletonAutoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    s_instance = go.AddComponent<T>();
                }
                return s_instance;
            }
        }
        
    }
}