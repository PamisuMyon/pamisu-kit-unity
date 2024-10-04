using Game.Configs;
using Game.Farm;
using PamisuKit.Framework;

namespace Game
{
    public class GameDirector : Director
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            CreateMonoSystem<ConfigSystem>();
            CreateMonoSystem<PatchSystem>();
        }
    }
}