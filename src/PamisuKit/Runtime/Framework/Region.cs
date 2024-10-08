using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class Region : MonoBehaviour
    {

        public Ticker Ticker { get; private set; }
        public IDirector Director { get; private set; }
        
        private readonly List<IEntity> _entities = new();

        public void Init(Ticker ticker, IDirector director)
        {
            Ticker = ticker;
            Director = director;
        }
        
        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            Ticker.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!_entities.Contains(entity))
                return;
            _entities.Remove(entity);
            Ticker.Remove(entity);
        }

        public TDirector GetDirector<TDirector>() where TDirector : class, IDirector
        {
            return Director as TDirector;
        }
        
    }
}