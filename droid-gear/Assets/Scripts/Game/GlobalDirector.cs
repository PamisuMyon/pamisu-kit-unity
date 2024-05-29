using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Input;
using PamisuKit.Framework;
using UnityEditor;

namespace Game
{
    public enum GameState
    {
        None,
        GlobalSystemsReady
    }

    public class GlobalDirector : Director<GlobalDirector>
    {
        public GameState GameState { get; private set; } = GameState.None;
        public static bool IsGlobalSystemsReady => Instance != null && Instance.GameState >= GameState.GlobalSystemsReady;

        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        public async UniTaskVoid Init()
        {
            CreateSystem<InputWrapper>();
            CreateSystem<ConfigSystem>();
            
            InputWrapper.Instance.Init();
            await ConfigSystem.Instance.Init();

            GameState = GameState.GlobalSystemsReady;
        }
        
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
    }
}