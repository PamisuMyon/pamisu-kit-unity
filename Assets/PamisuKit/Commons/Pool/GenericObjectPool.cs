using System.Collections.Generic;

namespace Pamisu.Commons.Pool
{

    public class ObjectWrapper<T>
    {
        public T Object { get; set; }

        public bool IsActive { get; set; }

        public void ToggleActivity(bool isActive)
        {
            IsActive = isActive;
        }

    }
    
    public class GenericObjectPool<T>
    {
        private List<ObjectWrapper<T>> list;
        private Dictionary<T, ObjectWrapper<T>> activeDict;
        private int lastIndex;

        public GenericObjectPool()
        {
            list = new List<ObjectWrapper<T>>();
            activeDict = new Dictionary<T, ObjectWrapper<T>>();
        }

        public void AddItem(T item, bool isActive = true)
        {
            var wrapper = new ObjectWrapper<T>
            {
                Object = item,
                IsActive = isActive,
            };
            list.Add(wrapper);
            if (isActive)
                activeDict.Add(item, wrapper);
        }

        public T GetItem()
        {
            for (var i = 0; i < list.Count; i++)
            {
                lastIndex++;
                if (lastIndex > list.Count - 1)
                    lastIndex = 0;

                if (!list[lastIndex].IsActive)
                {
                    list[lastIndex].ToggleActivity(true);
                    var item = list[lastIndex].Object;
                    activeDict.Add(item, list[lastIndex]);
                    return item;
                }
            }

            return default;
        }

        public void ReturnItem(T item)
        {
            if (activeDict.ContainsKey(item))
            { 
                activeDict[item].ToggleActivity(false);
                activeDict.Remove(item);
            }
        }
        
    }
    
}