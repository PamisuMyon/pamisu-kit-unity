using Game.Framework;
using Game.Save;

namespace Game.Worker.Models
{
    public class WorkerController : Unit, ISavable
    {
        public WorkerData Data { get; private set; }
        
        public void Init(WorkerData data)
        {
            Data = data;
            Trans.position = Data.Position;
            if (string.IsNullOrEmpty(Data.Id))
                Data.Id = GenerateId();
            Id = Data.Id;
        }

        public void OnSave(SaveData _)
        {
            Data.Position = Trans.position;
        }
    }
}