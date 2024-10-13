using Game.Farm;
using Game.Framework;
using Game.Inventory;
using Game.UI;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game
{
    public class GameDirector : Director
    {
        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private PlayerController _player;
        
        private Ticker _uiTicker;
        public Region UIRegion { get; private set; }
        public GameUI GameUI => _gameUI;
        public MonoPooler Pooler { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            var uiRegionGo = new GameObject("UIRegion");
            uiRegionGo.transform.SetParent(transform);
            _uiTicker = uiRegionGo.AddComponent<Ticker>();
            UIRegion = uiRegionGo.AddComponent<Region>();
            UIRegion.Init(_uiTicker, this);

            Pooler = new MonoPooler(Trans);

            CreateMonoSystem<InventorySystem>();
            CreateMonoSystem<PatchSystem>();
            
            if (_player != null)
                SetupMonoEntity(_player);
            
            if (_gameUI != null)
                _gameUI.Setup(UIRegion);
        }
        
    }
}