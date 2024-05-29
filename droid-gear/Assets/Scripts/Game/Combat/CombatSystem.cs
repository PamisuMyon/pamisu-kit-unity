using PamisuKit.Common.Pool;
using PamisuKit.Framework;

namespace Game.Combat
{
    public class CombatSystem : System<CombatSystem>
    {
        public MonoPooler Pooler { get; private set; }

        public void Init()
        {
            Pooler = new MonoPooler(Region.Trans);
        }

    }
}