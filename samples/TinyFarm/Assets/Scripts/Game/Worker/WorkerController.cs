using Game.Framework;

namespace Game.Worker.Models
{
    public class WorkerController : Unit
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

        public void UpdateData()
        {
            Data.Position = Trans.position;
        }
        
    }
}