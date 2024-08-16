using PamisuKit.Framework;

namespace Game.Save
{
    public class SaveSystem : System<SaveSystem>
    {
        public RuntimeData RuntimeData { get; private set;} = new();
    }
}
