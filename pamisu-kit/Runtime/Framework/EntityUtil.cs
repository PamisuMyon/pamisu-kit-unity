using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using PamisuKit.Common.Util;
using UnityEngine;

namespace PamisuKit.Framework
{
    public static class EntityUtil
    {
        public static async UniTask<T> InstantiateMonoEntity<T>(this Region region, object key, string name = null) where T : MonoEntity
        {
            var prefab = await AssetManager.LoadAsset<GameObject>(key);
            return InstantiateMonoEntity<T>(region, prefab, name);
        }

        public static T InstantiateMonoEntity<T>(this Region region, GameObject prefab, string name = null) where T : MonoEntity
        {
            var go = Object.Instantiate(prefab);
            if (name != null)
                go.name = name;
            var entity = go.GetOrAddComponent<T>();
            entity.Setup(region);
            return entity;
        }
        
        public static T NewMonoEntity<T>(this Region region, string name = null) where T : MonoEntity
        {
            var go = new GameObject();
            name ??= typeof(T).Name;
            go.name = name;
            var entity = go.GetOrAddComponent<T>();
            entity.Setup(region);
            return entity;
        }
        
    }
}