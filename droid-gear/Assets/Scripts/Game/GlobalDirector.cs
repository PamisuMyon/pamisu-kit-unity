using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Input;
using PamisuKit.Framework;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public enum GameState
    {
        None,
        GlobalSystemsReady
    }

    public class GlobalDirector : Director
    {
        public static GlobalDirector Instance { get; private set; }
        public static bool IsGlobalSystemsReady => Instance != null && Instance.GameState >= GameState.GlobalSystemsReady;

        [SerializeField]
        private bool _dontDestroyOnLoad = false;

        public GameState GameState { get; private set; } = GameState.None;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<GlobalDirector>();
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            base.Awake();
        }

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