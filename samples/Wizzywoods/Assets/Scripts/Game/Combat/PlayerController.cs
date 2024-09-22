using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Framework;
using PamisuKit.Framework;

namespace Game.Combat
{
    public class PlayerController : MonoEntity
    {
        public Character Chara { get; private set; }

        public void Init(Character character)
        {
            Chara = character;
            On<RequestActivatePlayerAbility>(OnRequestActivatePlayerAbility);
        }
        
        private void OnRequestActivatePlayerAbility(RequestActivatePlayerAbility e)
        {
            Chara.AbilityComp.TryActiveAbility(e.Id).Forget();
        }
        
    }
}