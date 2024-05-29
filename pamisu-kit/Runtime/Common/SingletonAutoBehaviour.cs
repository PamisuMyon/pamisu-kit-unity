using UnityEngine;

namespace PamisuKit.Common
{
    public class SingletonAutoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject();
                    go.name = typeof(T).ToString();
                    _instance = go.AddComponent<T>();
                }
                return _instance;
            }
        }
        
    }
}