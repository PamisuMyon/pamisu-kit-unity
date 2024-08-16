using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Input;
using Game.Save;
using PamisuKit.Framework;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class AppDirector : Director
    {
        public static AppDirector Instance { get; private set; }

        [SerializeField]
        private bool _dontDestroyOnLoad = false;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = GetComponent<AppDirector>();
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
            CreateMonoSystem<SaveSystem>();
            CreateMonoSystem<InputWrapper>();
            CreateMonoSystem<ConfigSystem>();
            
            InputWrapper.Instance.Init();
            await ConfigSystem.Instance.Init();

            SaveSystem.Instance.RuntimeData.GameState = GameState.AppReady;
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