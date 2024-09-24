using Game.Configs;
using Game.Input;
using Game.Save;
using PamisuKit.Framework;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Game
{
    public class App : AppDirector<App>
    {

        protected override void OnCreate()
        {
            Init();
            base.OnCreate();
        }

        private void Init()
        {
            CreateMonoSystem<ConfigSystem>();
            CreateMonoSystem<SaveSystem>();
            CreateMonoSystem<InputWrapper>();
        }

        public void LoadTitle()
        {
            SceneManager.LoadScene(0);
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