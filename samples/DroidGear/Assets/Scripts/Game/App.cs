using Game.Input;
using Game.Save;
using PamisuKit.Framework;
using UnityEditor;

namespace Game
{
    public class App : BaseApp<App>
    {

        protected override void OnCreate()
        {
            Init();
            base.OnCreate();
        }

        public void Init()
        {
            CreateMonoSystem<SaveSystem>();
            CreateMonoSystem<InputWrapper>();
            
            GetSystem<InputWrapper>().Init();
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