using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class Region
    {

        public Transform Trans { get; private set; }
        public GameObject Go { get; private set; }
        public Ticker Ticker { get; private set; }
        public Director Director { get; private set; }
        
        private readonly List<IEntity> _entities = new();

        public Region(Ticker ticker, Director director, string name = null)
        {
            Ticker = ticker;
            Director = director;
            name ??= GetType().Name;
            Go = new GameObject();
            Go.name = name;
            Trans = Go.transform;
            Trans.SetParent(director.transform); 
        }

        // public Region(Ticker ticker, Transform parent = null, string name = null)
        // {
        //     Ticker = ticker;
        //     name ??= GetType().Name;
        //     Go = new GameObject();
        //     Go.name = name;
        //     Trans = Go.transform;
        //     Trans.SetParent(parent);
        // }
        
        // public Region(Transform parent = null, string name = null)
        // {
        //     name ??= GetType().Name;
        //     Go = new GameObject();
        //     Go.name = name;
        //     Trans = Go.transform;
        //     Trans.SetParent(parent);
        // }
        
        public void AddEntity(IEntity entity, bool reparent = true)
        {
            _entities.Add(entity);
            if (reparent && entity.Trans != null)
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

        public TDirector GetDirector<TDirector>() where TDirector : Director
        {
            return Director as TDirector;
        }
        
    }
}