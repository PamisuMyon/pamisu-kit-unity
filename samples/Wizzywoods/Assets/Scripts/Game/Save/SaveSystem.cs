using Game.Save.Models;
using PamisuKit.Framework;

namespace Game.Save
{
    public class SaveSystem : MonoSystem
    {
        
        public RuntimeData RuntimeData { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            RuntimeData = new RuntimeData();
        }
        
    }
}