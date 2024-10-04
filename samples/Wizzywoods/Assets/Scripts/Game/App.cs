using Game.Configs;
using Game.Save;
using PamisuKit.Framework;
using UnityEditor;

namespace Game
{
    public class App : AppDirector<App>
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            
            CreateMonoSystem<ConfigSystem>();
            CreateMonoSystem<SaveSystem>();
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