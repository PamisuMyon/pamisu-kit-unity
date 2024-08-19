using Cysharp.Threading.Tasks;
using Game.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Combat
{
    public class CombatDirector : GameDirector
    {
        public bool IsReady { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            if (GameApp.Instance.IsReady)
                Init();
            else
                GameApp.Instance.Ready += OnAppReady;
        }

        private void OnAppReady() => Init();

        protected void Init()
        {
            CreateMonoSystem<CombatSystem>();
            IsReady = true;
            
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

    }
}