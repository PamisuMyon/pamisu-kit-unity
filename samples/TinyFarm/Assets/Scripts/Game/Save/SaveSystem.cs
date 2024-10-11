using Cysharp.Threading.Tasks;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Save
{
    // TODO Multiple user profile support
    public class SaveSystem : MonoSystem, IUpdatable
    {

        private SaveHandler _saveHandler;
        
        public bool IsSaving { get; private set; }
        public SaveData SaveData { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _saveHandler = new SaveHandler(Application.persistentDataPath);
        }

        public async UniTask Init()
        {
            await LoadSaveFromLocal();
        }

        private async UniTask LoadSaveFromLocal()
        {
            SaveData = await _saveHandler.Load<SaveData>();
            if (SaveData == null)
                SaveData = new SaveData();
            SaveData.PostDeserialize();
        }

        public void OnUpdate(float deltaTime)
        {
            // TODO TEMP
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                RequestSaveAll();
                Debug.Log("Debug saved");
            }
        }
        
        private void RequestSaveAll()
        {
            if (IsSaving)
                return;
            SaveAll().Forget();
        }
        
        private async UniTaskVoid SaveAll()
        {
            IsSaving = true;
            SaveData.PreSerialize();
            await _saveHandler.Save(SaveData);
            IsSaving = false;
        }
        
    }
}