using Cysharp.Threading.Tasks;
using Game.Framework;
using Game.Save;
using PamisuKit.Framework;
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
            Init().Forget();
        }

        protected async UniTaskVoid Init()
        {
            if (!SaveSystem.Instance.RuntimeData.IsAppReady)
                await UniTask.WaitUntil(() => SaveSystem.Instance.RuntimeData.IsAppReady);
            
            CreateMonoSystem<CombatSystem>();

            IsReady = true;
            SaveSystem.Instance.RuntimeData.GameState = GameState.Combat;

            CombatSystem.Instance.StartCombat();
        }

#if UNITY_EDITOR
        protected override void Update()
        {
            base.Update();
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