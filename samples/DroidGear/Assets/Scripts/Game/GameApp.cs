using System;
using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Save;
using PamisuKit.Framework;
using UnityEditor;

namespace Game
{
    public class GameApp : BaseApp<GameApp>
    {

        public bool IsReady { get; private set; }
        public event Action Ready;

        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        public async UniTaskVoid Init()
        {
            CreateMonoSystem<SaveSystem>();
            CreateMonoSystem<InputWrapper>();
            // CreateMonoSystem<ConfigSystem>();
            
            GetSystem<InputWrapper>().Init();
            // await GetSystem<ConfigSystem>().Init();

            IsReady = true;
            Ready?.Invoke();
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