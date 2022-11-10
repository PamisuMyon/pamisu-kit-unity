using System.Collections.Generic;
using UnityEngine;

namespace Pamisu.Commons
{
    public static class DontDestroyOnLoadManager
    {
        private static List<GameObject> s_objects = new();

        public static void DontDestroyOnLoad(this GameObject go)
        {
            Object.DontDestroyOnLoad(go);
            s_objects.Add(go);
        }

        public static void DestroyAll()
        {
            foreach (var go in s_objects)
                if (go != null)
                    Object.Destroy(go);
            s_objects.Clear();
        }
    }
}