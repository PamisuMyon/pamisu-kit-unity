using Game.Farm;
using Game.UI;
using PamisuKit.Framework;
using UnityEngine;

namespace Game
{
    public class GameDirector : Director
    {
        [SerializeField]
        private GameUI _gameUI;
        
        private Ticker _uiTicker;
        public Region UIRegion { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            var uiRegionGo = new GameObject("UIRegion");
            uiRegionGo.transform.SetParent(transform);
            _uiTicker = uiRegionGo.AddComponent<Ticker>();
            UIRegion = uiRegionGo.AddComponent<Region>();
            UIRegion.Init(_uiTicker, this);
            
            CreateMonoSystem<PatchSystem>();
            
            if (_gameUI != null)
                _gameUI.Setup(UIRegion);
        }
        
    }
}