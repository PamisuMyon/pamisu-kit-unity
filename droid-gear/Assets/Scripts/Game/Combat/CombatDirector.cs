using Cysharp.Threading.Tasks;
using PamisuKit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Combat
{
    public class CombatDirector : Director<CombatDirector>
    {
        
        public static bool IsReady => Instance != null && Instance._isReady;
        private bool _isReady;

        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        protected async @bool Init()
        {
            if (!GlobalDirector.IsGlobalSystemsReady)
                await UniTask.WaitUntil(() => GlobalDirector.IsGlobalSystemsReady);
            
            CreateSystem<CombatSystem>();
            CombatSystem.Instance.Init();

            _isReady = true;
        }

#if UNITY_EDITOR
        protected override void Update()
        {
            base.Update();
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
#endif

    }
}