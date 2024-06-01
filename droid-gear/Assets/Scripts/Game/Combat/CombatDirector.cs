using Cysharp.Threading.Tasks;
using Game.Framework;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Combat
{
    public class CombatDirector : GameDirector
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        protected async UniTaskVoid Init()
        {
            if (!GlobalDirector.IsGlobalSystemsReady)
                await UniTask.WaitUntil(() => GlobalDirector.IsGlobalSystemsReady);
            
            CreateMonoSystem<CombatSystem>();

            CombatSystem.Instance.StartCombat().Forget();
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