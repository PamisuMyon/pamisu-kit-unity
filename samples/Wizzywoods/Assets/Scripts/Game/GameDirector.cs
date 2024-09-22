using Cysharp.Threading.Tasks;
using Game.Save;
using Game.Save.Models;
using PamisuKit.Framework;

namespace Game.Combat
{
    public class GameDirector : Director
    {
        
        protected override void OnCreate()
        {
            base.OnCreate();
            CreateMonoSystem<CombatSystem>();
        }

    }
}