using System.Collections.Generic;
using Game.AI;
using Game.AI.Actions;
using Game.Farm;
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
        private const string KeyCurrentTaskType = "KeyCurrentTaskType";
        private const string KeyTargetPos = "KeyTargetPos";
        private const string KeyTargetTrans = "KeyTargetTrans";

        [Header("Worker")]
        [SerializeField]
        private float _moveSpeed = 5f;

        [Header("Idle")]
        [SerializeField]
        private Vector2 _idleDurationRange = new(1, 6);

        [SerializeField]
        private Vector2 _wanderDistanceRange = new(2, 8);

        private SpriteRenderer _spriteRenderer;
        private NavMeshAgent _navAgent;
        private Blackboard _blackboard;
        private Root _behaviourTree;
        private WorkerSystem _workerSystem;
        private List<WorkerTaskType> _taskPriorities;

        private int _faceDirection;

        public int FaceDirection
        {
            get => _faceDirection;
            set
            {
                _faceDirection = value;
                _spriteRenderer.flipX = _faceDirection == -1;
            }
        }

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
            _blackboard[KeyCurrentTaskType] = WorkerTaskType.None;
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
                    Water(),
                    Idle()
                )
            );
        }

        private Node Water()
        {
            return new BlackboardCondition(
                KeyCurrentTaskType, Operator.IS_EQUAL, WorkerTaskType.Watering,
                Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                new Sequence(
                    new NavMoveToTarget(_navAgent, KeyTargetTrans),
                    new Action(StopNavAgent),
                    new Action(LookAtTarget),
                    new Action(WaterTarget),
                    new Action(CompleteCurrentTask)
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

        private void CompleteCurrentTask()
        {
            Data.CurrentTask = null;
            _blackboard[KeyTargetTrans] = null;
        }

        private void LookAtTarget()
        {
            var target = _blackboard[KeyTargetTrans] as Transform;
            FaceDirection = (int)Mathf.Sign(target.position.x - Trans.position.x);
        }

        private void WaterTarget()
        {
            var target = Data.CurrentTask.GetTarget(this) as Plot;
            Debug.Assert(target != null, "Water target is not Plot", Go);
            target.Water();
        }

        private void UpdateWorkerTask()
        {
            for (int i = 0; i < _taskPriorities.Count; i++)
            {
                var task = _workerSystem.GetPendingTask(_taskPriorities[i]);
                if (task != null)
                {
                    Data.CurrentTask = task;
                    _blackboard[KeyCurrentTaskType] = task.Type;
                    _blackboard[KeyTargetTrans] = task.GetTarget(this).Trans;
                    break;
                }
            }
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