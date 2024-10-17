using System.Collections.Generic;
using Game.AI;
using Game.AI.Actions;
using Game.Framework;
using Game.Save;
using NPBehave;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Worker.Models
{
    public class WorkerController : Unit, ISavable
    {
        private const string KeyTargetPos = "KeyTargetPos";
        private const string KeyTargetUnit = "KeyTargetUnit";

        [Header("Worker")]
        [SerializeField]
        private float _moveSpeed = 5f;

        [Header("Idle")]
        [SerializeField]
        private Vector2 _idleDurationRange = new(1, 6);

        [SerializeField]
        private Vector2 _wanderDistanceRange = new(2, 8);

        
        private NavMeshAgent _navAgent;
        private Blackboard _blackboard;
        private Root _behaviourTree;
        private WorkerSystem _workerSystem;
        private List<WorkerTaskType> _taskPriorities;

        public WorkerData Data { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _navAgent = GetComponent<NavMeshAgent>();
            _navAgent.updateRotation = false;
            _navAgent.updateUpAxis = false;
            _navAgent.enabled = true;
            
            _workerSystem = GetSystem<WorkerSystem>();
            _taskPriorities = new List<WorkerTaskType>()
            {
                WorkerTaskType.Construction,
                WorkerTaskType.Harvesting,
                WorkerTaskType.Watering,
            };

            var clock = GetDirector<GameDirector>().BTClock;
            _blackboard = new Blackboard(clock);
            _behaviourTree = CreateBehaviourTree(_blackboard, clock);
        }

        public void Init(WorkerData data)
        {
            Data = data;
            Trans.position = Data.Position;
            if (string.IsNullOrEmpty(Data.Id))
                Data.Id = GenerateId();
            Id = Data.Id;

            _behaviourTree.Start();
        }

        public void OnSave(SaveData _)
        {
            Data.Position = Trans.position;
        }

        #region Behaviour Tree

        private Root CreateBehaviourTree(Blackboard blackboard, Clock clock)
        {
            return new Root(blackboard, clock,
                new Selector(
                    Idle()
                )
            );
        }

        private Node Idle()
        {
            return new Service(0.5f, UpdateWorkerTask,
                new Sequence(
                    new Action(StopNavAgent),
                    new Wait(Random.Range(_idleDurationRange.x, _idleDurationRange.y)),
                    new Action(PickWanderPos),
                    new NavMoveToTarget(_navAgent, KeyTargetPos)
                )
            );
        }

        private void UpdateWorkerTask()
        {
            
        }

        private void PickWanderPos()
        {
            AIUtil.SetRandomPositionOnNavMesh(_blackboard, KeyTargetPos, Trans.position, Random.Range(_wanderDistanceRange.x, _wanderDistanceRange.y));
        }

        private void StopNavAgent()
        {
            if (_navAgent.enabled)
            {
                _navAgent.isStopped = true;
            }
        }

        #endregion
    }
}