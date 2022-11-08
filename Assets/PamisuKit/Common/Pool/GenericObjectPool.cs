using System.Collections.Generic;

namespace Pamisu.Common.Pool
{

    public class ObjectWrapper<T>
    {
        private T _object;

        public T Object
        {
            get => _object;
            set => _object = value;
        }
        
        public bool IsActive { get; set; }

        public void ToggleActivity(bool isActive)
        {
            IsActive = isActive;
        }

    }
    
    public class GenericObjectPool<T>
    {
        private List<ObjectWrapper<T>> _list;
        private Dictionary<T, ObjectWrapper<T>> _activeDict;
        private int _lastIndex;

        public GenericObjectPool()
        {
            _list = new List<ObjectWrapper<T>>();
            _activeDict = new Dictionary<T, ObjectWrapper<T>>();
        }

        public void AddItem(T item, bool isActive = true)
        {
            var wrapper = new ObjectWrapper<T>
            {
                Object = item,
                IsActive = isActive,
            };
            _list.Add(wrapper);
            if (isActive)
                _activeDict.Add(item, wrapper);
        }

        public T GetItem()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                _lastIndex++;
                if (_lastIndex > _list.Count - 1)
                    _lastIndex = 0;

                if (!_list[_lastIndex].IsActive)
                {
                    _list[_lastIndex].ToggleActivity(true);
                    var item = _list[_lastIndex].Object;
                    _activeDict.Add(item, _list[_lastIndex]);
                    return item;
                }
            }

            return default;
        }

        public void ReturnItem(T item)
        {
            if (_activeDict.ContainsKey(item))
            { 
                _activeDict[item].ToggleActivity(false);
                _activeDict.Remove(item);
            }
        }
        
    }
    
}