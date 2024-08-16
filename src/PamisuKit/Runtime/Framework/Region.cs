using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class Region : MonoBehaviour
    {

        public Transform Trans { get; private set; }
        public GameObject Go { get; private set; }
        public Ticker Ticker { get; private set; }
        public Director Director { get; private set; }
        
        private readonly List<IEntity> _entities = new();

        public void Init(Ticker ticker, Director director)
        {
            Ticker = ticker;
            Director = director;
        }
        
        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            if (entity.Trans != null && entity.Trans.parent == null)
                entity.Trans.SetParent(Trans);
            Ticker.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!_entities.Contains(entity))
                return;
            _entities.Remove(entity);
            Ticker.Remove(entity);
        }

        public TDirector GetDirector<TDirector>() where TDirector : Director
        {
            return Director as TDirector;
        }
        
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return Director.GetSystem<TSystem>();
        }
        
        public ISystem GetSystem(Type type)
        {
            return Director.GetSystem(type);
        }
        
    }
}