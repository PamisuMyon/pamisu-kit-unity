using PamisuKit.Framework;

namespace Game.Save
{
    public class SaveSystem : MonoSystem
    {
        public RuntimeData RuntimeData { get; private set;} = new();
    }
}
