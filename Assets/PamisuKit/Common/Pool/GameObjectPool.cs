using System.Collections.Generic;
using UnityEngine;

namespace Pamisu.Common.Pool
{
    public class GameObjectPool
    {
        private List<GameObject> _list;
        private int _lastIndex;

        public GameObjectPool()
        {
            _list = new List<GameObject>();
            _lastIndex = 0;
        }

        public void AddItem(GameObject gameObject)
        {
            _list.Add(gameObject);
        }

        public GameObject GetItem()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                _lastIndex++;
                if (_lastIndex > _list.Count - 1)
                    _lastIndex = 0;

                if (!_list[_lastIndex].activeInHierarchy)
                {
                    return _list[_lastIndex];
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

        private static GameObjectPool s_pool = new();
        
        public static GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            var go = s_pool.GetItem();
            if (go == null)
            {
                go = Instantiate(prefab);
                s_pool.AddItem(go);
            }
            if (position != default)
                go.transform.position = position;
            if (rotation != default)
                go.transform.rotation = rotation;
            
            return go;
        }

        public static void Return(GameObject go)
        {
            s_pool.ReturnItem(go);
        }
        
    }
}