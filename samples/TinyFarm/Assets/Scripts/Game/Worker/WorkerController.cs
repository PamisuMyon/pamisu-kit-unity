using PamisuKit.Framework;

namespace Game.Worker.Models
{
    public class WorkerController : MonoEntity
    {
        public WorkerData Data { get; private set; }
        
        public void Init(WorkerData data)
        {
            Data = data;
            Trans.position = data.Position;
        }

        public void UpdateData()
        {
            Data.Position = Trans.position;
        }
        
    }
}