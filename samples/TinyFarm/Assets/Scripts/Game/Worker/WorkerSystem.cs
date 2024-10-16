using System.Collections.Generic;
using Game.Events;
using Game.Save;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Worker.Models
{
    public class WorkerSystem : MonoSystem, ISavable
    {
        [SerializeField]
        private int _initWorkerNum;

        [SerializeField]
        private float _workerSpawnRadius;

        [SerializeField]
        private GameObject _workerPrefab;
        
        private SaveSystem _saveSystem;

        public WorkerSystemData Data => _saveSystem.SaveData.Worker;
        public Dictionary<WorkerTaskType, Queue<WorkerTask>> TaskQueueDict => Data.TaskQueueDict;
        public List<WorkerData> WorkerDataList => Data.Workers;
        public List<WorkerController> Workers;

        protected override void OnCreate()
        {
            base.OnCreate();
            _saveSystem = GetSystem<SaveSystem>();
            _saveSystem.RegisterSavable(this);

            if (WorkerDataList.Count != 0)
            {
                for (int i = 0; i < _initWorkerNum; i++)
                {
                    var data = new WorkerData
                    {
                        Position = Random.insideUnitCircle * _workerSpawnRadius,
                    };
                    WorkerDataList.Add(data);
                }
            }
            for (int i = 0; i < WorkerDataList.Count; i++)
            {
                Workers.Add(SpawnWorker(WorkerDataList[i]));
            }

            On<ReqAddWorkerTask>(OnReqAddWorkerTask);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            if (_saveSystem != null)
                _saveSystem.RemoveSavable(this);
        }
        
        public void OnSave(SaveData saveData)
        {
            for (int i = 0; i < Workers.Count; i++)
            {
                Workers[i].OnSave(saveData);
            }
        }
        
        private void OnReqAddWorkerTask(ReqAddWorkerTask e)
        {
            var task = new WorkerTask(e.Target, e.Type);
            if (!TaskQueueDict.ContainsKey(e.Type))
                TaskQueueDict[e.Type] = new Queue<WorkerTask>();
            TaskQueueDict[e.Type].Enqueue(task);
        }

        private WorkerController SpawnWorker(WorkerData workerData)
        {
            var go = Instantiate(_workerPrefab);
            var worker = go.GetComponent<WorkerController>();
            worker.Setup(Region);
            worker.Init(workerData);
            return worker;
        }
        
    }
}