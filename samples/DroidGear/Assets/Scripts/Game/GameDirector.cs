using Game.Combat;
using Game.UI.Combat;
using Game.Upgrades;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameDirector : Director
    {
        [SerializeField]
        private CombatUI _combatUI;

        private Ticker _uiTicker;
        public Region UIRegion { get; private set; }
        public MonoPooler Pooler { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            var uiRegionGo = new GameObject("UIRegion");
            uiRegionGo.transform.SetParent(transform);
            _uiTicker = uiRegionGo.AddComponent<Ticker>();
            UIRegion = uiRegionGo.AddComponent<Region>();
            UIRegion.Init(_uiTicker, this);
            
            Pooler = new MonoPooler(transform);
            
            CreateMonoSystem<CombatSystem>();
            CreateMonoSystem<UpgradeSystem>();
            
            if (_combatUI != null)
                _combatUI.Setup(UIRegion);
            
            GetSystem<CombatSystem>().StartCombat();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Debug.Log("Reload current scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                Ticker.TimeScale = Mathf.Min(2, Ticker.TimeScale + 0.1f);
                Debug.Log($"Ticker timescale: {Ticker.TimeScale}");
            }
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                Ticker.TimeScale = Mathf.Max(0, Ticker.TimeScale - 0.1f);
                Debug.Log($"Ticker timescale: {Ticker.TimeScale}");
            }
        }
#endif

        public void PauseCombat()
        {
            Region.Ticker.TimeScale = 0;
        }
        
        public void ResumeCombat()
        {
            Region.Ticker.TimeScale = 1f;
        }

    }
}