using PamisuKit.Framework;

namespace Game.Save
{
    public class SaveSystem : MonoSystem
    {
        
        public SaveData SaveData { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            // TODO TEMP
            SaveData = new SaveData();
            SaveData.PostDeserialize();
        }
        
    }
}