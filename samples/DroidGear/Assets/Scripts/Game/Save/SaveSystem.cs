using PamisuKit.Framework;

namespace Game.Save
{
    public class SaveSystem : MonoSystem<SaveSystem>
    {
        public RuntimeData RuntimeData { get; private set;} = new();
    }
}
