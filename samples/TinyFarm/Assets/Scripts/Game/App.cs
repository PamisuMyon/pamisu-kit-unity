using Game.Configs;
using Game.Inputs;
using Game.Save;
using PamisuKit.Framework;

namespace Game
{
    public class App : AppDirector<App>
    {
        protected override void OnCreate()
        {
            CreateMonoSystem<InputSystem>();
            CreateMonoSystem<ConfigSystem>();
            CreateMonoSystem<SaveSystem>();
            
            base.OnCreate();
        }
        
    }
}