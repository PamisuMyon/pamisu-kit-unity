using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class Region
    {

        public Ticker Ticker { get; protected set; }
        public Transform Trans { get; protected set; }
        public GameObject Go { get; protected set; }
        
        private readonly List<IEntity> _entities = new();

        public Region(Ticker ticker, Transform parent = null, string name = null)
        {
            Ticker = ticker;
            name ??= GetType().Name;
            Go = new GameObject();
            Go.name = name;
            Trans = Go.transform;
            Trans.SetParent(parent);
        }
        
        public Region(Transform parent = null, string name = null)
        {
            name ??= GetType().Name;
            Go = new GameObject();
            Go.name = name;
            Trans = Go.transform;
            Trans.SetParent(parent);
        }
        
        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            if (entity.Trans != null)
                entity.Trans.SetParent(Trans);
            Ticker?.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!_entities.Contains(entity))
                return;
            _entities.Remove(entity);
            Ticker?.Remove(entity);
        }
        
    }
}