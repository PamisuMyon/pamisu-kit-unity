using System.Collections.Generic;
using UnityEngine;

namespace Pamisu.Commons.Pool
{
    public class GameObjectPool
    {
        private List<GameObject> list;
        private int lastIndex;

        public GameObjectPool()
        {
            list = new List<GameObject>();
            lastIndex = 0;
        }

        public void AddItem(GameObject gameObject)
        {
            list.Add(gameObject);
        }

        public GameObject GetItem()
        {
            for (var i = 0; i < list.Count; i++)
            {
                lastIndex++;
                if (lastIndex > list.Count - 1)
                    lastIndex = 0;

                if (!list[lastIndex].activeInHierarchy)
                {
                    return list[lastIndex];
                }
            }

            return null;
        }

        public void ReturnItem(GameObject item)
        {
            item.SetActive(false);
        }
        
    }

    public class GameObjectPooler : SingletonAutoBehaviour<GameObjectPooler>
    {

        private readonly Dictionary<GameObject, GameObjectPool> pools = new();

        private void PrewarmInterval(GameObject prefab, int size = 1)
        {
            if (pools.ContainsKey(prefab)) return;
            var pool = new GameObjectPool();
            for (var i = 0; i < size; i++)
            {
                var go = Instantiate(prefab);
                go.SetActive(false);
                pool.AddItem(go);
            }
            pools.Add(prefab, pool);
        }
        
        private GameObject SpawnInterval(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            if (!pools.ContainsKey(prefab))
                PrewarmInterval(prefab);

            var pool = pools[prefab];
            var go = pool.GetItem();
            if (go == null)
            {
                go = Instantiate(prefab);
                pool.AddItem(go);
            }
            go.SetActive(true);
            if (position != default)
                go.transform.position = position;
            if (rotation != default)
                go.transform.rotation = rotation;
            
            return go;
        }

        private void ReleaseInterval(GameObject go)
        {
            go.SetActive(false);
        }

        public static void Prewarm(GameObject prefab, int size = 1) => Instance.PrewarmInterval(prefab, size);

        public static GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            return Instance.SpawnInterval(prefab, position, rotation);
        }

        public static void Release(GameObject go) => Instance.ReleaseInterval(go);

    }
}