using System.Collections.Generic;
using Game.Buildings;
using Game.Farm;
using Game.Framework;
using Game.Inventory;
using Game.UI;
using Game.Worker.Models;
using NPBehave;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game
{
    public class GameDirector : Director, IUpdatable
    {
        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private PlayerController _player;
        
        private readonly Queue<MonoEntity> _entitySetupQueue = new();
        
        private Ticker _uiTicker;
        public Region UIRegion { get; private set; }
        public GameUI GameUI => _gameUI;
        public MonoPooler Pooler { get; private set; }
        public Clock BTClock { get; private set; }
        public bool IsActive => Go.activeInHierarchy;
        public bool IsReady { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            Region.Ticker.Add(this);
            
            var uiRegionGo = new GameObject("UIRegion");
            uiRegionGo.transform.SetParent(transform);
            _uiTicker = uiRegionGo.AddComponent<Ticker>();
            UIRegion = uiRegionGo.AddComponent<Region>();
            UIRegion.Init(_uiTicker, this);

            Pooler = new MonoPooler(Trans);
            BTClock = new Clock();

            CreateMonoSystem<InventorySystem>();
            CreateMonoSystem<PatchSystem>();
            CreateMonoSystem<BuildingSystem>();
            CreateMonoSystem<WorkerSystem>();
            
            if (_player != null)
                SetupMonoEntity(_player);
            
            if (_gameUI != null)
                _gameUI.Setup(UIRegion);
            
            while (_entitySetupQueue.Count > 0)
            {
                _entitySetupQueue.Dequeue().Setup(Region);
            }
            IsReady = true;
        }
        
        public override void SetupMonoEntity(MonoEntity entity)
        {
            if (!IsReady)
            {
                _entitySetupQueue.Enqueue(entity);
                return;
            }
            entity.Setup(Region);
        }

        public void OnUpdate(float deltaTime)
        {
            BTClock.Update(deltaTime);
        }
    }
}