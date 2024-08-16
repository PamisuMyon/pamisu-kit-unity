using PamisuKit.Common.Pool;
using PamisuKit.Framework;

namespace Game.Framework
{
    public abstract class GameDirector : Director
    {
        public MonoPooler Pooler { get; protected set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Pooler = new MonoPooler(Region.Trans);
        }
    }
}