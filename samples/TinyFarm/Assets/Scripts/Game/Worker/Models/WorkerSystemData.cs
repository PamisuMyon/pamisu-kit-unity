using System.Collections.Generic;
using Game.Framework;

namespace Game.Worker.Models
{
    public class WorkerSystemData : ISerializee
    {
        public Dictionary<WorkerTaskType, Queue<WorkerTask>> TaskQueueDict;
        public List<WorkerData> Workers;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            TaskQueueDict ??= new Dictionary<WorkerTaskType, Queue<WorkerTask>>();
            Workers ??= new List<WorkerData>();
        }
    }
}