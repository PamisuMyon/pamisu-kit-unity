using System.Collections.Generic;
using Game.AI;
using Game.AI.Actions;
using Game.Buildings;
using Game.Farm;
using Game.Framework;
using Game.Save;
using NPBehave;
using PamisuKit.Common.Util;
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
        private const string KeyTargetUnit = "KeyTargetUnit";

        [Header("Worker")]
        [SerializeField]
        private float _moveSpeed = 5f;

        [Header("Idle")]
        [SerializeField]
        private Vector2 _idleDurationRange = new(0, 6);

        [SerializeField]
        private Vector2 _wanderDistanceRange = new(2, 8);

        [SerializeField]
        private Transform _carrySlot;

        private WorkerCosmetic _cosmetic;
        private Blackboard _blackboard;
        private Root _behaviourTree;
        private WorkerSystem _workerSystem;
        private List<WorkerTaskType> _taskPriorities;
        private Produce _carryingProduce;

        public NavMeshAgent NavAgent { get; private set; }
        public WorkerData Data { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _cosmetic = GetComponent<WorkerCosmetic>();
            _cosmetic.Setup(Region);
            _cosmetic.Controller = this;

            NavAgent = GetComponent<NavMeshAgent>();
            NavAgent.updateRotation = false;
            NavAgent.updateUpAxis = false;
            NavAgent.enabled = true;

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
            
#if UNITY_EDITOR
            var debugger = Go.AddComponent<Debugger>();
            debugger.BehaviorTree = _behaviourTree;
#endif
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
                    Harvest(),
                    Water(),
                    Idle()
                )
            );
        }

        private Node Harvest()
        {
            return new BlackboardCondition(
                KeyCurrentTaskType, Operator.IS_EQUAL, WorkerTaskType.Harvesting,
                Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                new Sequence(
                    NavMoveToTargetWithAnim(KeyTargetPos),
                    new Action(LookAtTarget),
                    new HarvestPlot(this, KeyTargetUnit),
                    new Selector(
                        new Condition(() => _carryingProduce != null,
                            new Sequence(
                                new Action(PickWarehouseAsTarget),
                                NavMoveToTargetWithAnim(KeyTargetTrans, true),
                                new Action(StoreProduce),
                                new Action(CompleteCurrentTask),
                                new Action(UpdateWorkerTask)
                            )
                        ),
                        new Sequence(
                            new Action(CompleteCurrentTask),
                            new Action(UpdateWorkerTask)
                        )
                    )
                )
            );
        }

        private Node Water()
        {
            return new BlackboardCondition(
                KeyCurrentTaskType, Operator.IS_EQUAL, WorkerTaskType.Watering,
                Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                new Sequence(
                    NavMoveToTargetWithAnim(KeyTargetPos),
                    new Action(LookAtTarget),
                    new Action(_cosmetic.PlayWateringAnim),
                    new Action(WaterTarget),
                    new Action(CompleteCurrentTask)
                    // new Action(UpdateWorkerTask)
                )
            );
        }

        private Node Idle()
        {
            return new Service(0.5f, UpdateWorkerTask,
                new Sequence(
                    new Action(StopNavAgent),
                    new Action(_cosmetic.PlayIdleAnim),
                    new Wait(Random.Range(_idleDurationRange.x, _idleDurationRange.y)),
                    new Action(PickWanderPos),
                    NavMoveToTargetWithAnim(KeyTargetPos)
                )
            );
        }

        private void CompleteCurrentTask()
        {
            Data.CurrentTask = null;
            _blackboard[KeyCurrentTaskType] = WorkerTaskType.None;
            _blackboard[KeyTargetTrans] = null;
            _blackboard[KeyTargetUnit] = null;
        }

        private void LookAtTarget()
        {
            if (_blackboard[KeyTargetUnit] is Unit unit)
            {
                _cosmetic.FaceDirection = (int)Mathf.Sign(unit.Trans.position.x - Trans.position.x);
            }
            else
                Debug.LogError("LookAtTarget KeyTargetUnit is not unit", Go);
        }

        public void CarryProduce(Produce produce)
        {
            _carryingProduce = produce;
            produce.Trans.SetParent(_carrySlot);
            produce.Trans.localPosition = Vector3.zero;
        }

        public void StoreProduce()
        {
            Debug.Assert(_carryingProduce != null, "_carryingProduce != null", Go);
            // TODO TEMP
            GetDirector<GameDirector>().Pooler.Release(_carryingProduce);
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
                    var target = task.GetTarget(this);
                    Debug.Log($"Worker get task {task.Type} {target.name}", Go);
                    _blackboard[KeyCurrentTaskType] = task.Type;
                    _blackboard[KeyTargetUnit] = target;
                    _blackboard[KeyTargetTrans] = target.Trans;
                    if (target is Plot)
                    {
                        var pos = target.Trans.position;
                        var dir = pos - Trans.position;
                        var targetPos = pos - Vector3.right * (Mathf.Sign(dir.x) * target.VisualSize.x);
                        // When the target's X coordinate is close to the current position, the speed of NavMeshAgent will become very slow. This is a hack to solve the problem
                        if (targetPos.x.Approximately(Trans.position.x))
                        {
                            targetPos.x += RandomUtil.RandomSigned(0.05f, 0.1f);
                        }
                        _blackboard[KeyTargetPos] = targetPos;
                    }
                    else
                    {
                        _blackboard[KeyTargetTrans] = target.Trans;
                    }

                    break;
                }
            }
        }

        private void PickWarehouseAsTarget()
        {
            // TODO TEMP
            var buildingSystem = GetSystem<BuildingSystem>();
            _blackboard[KeyTargetUnit] = buildingSystem.Warehouse;
            _blackboard[KeyTargetTrans] = buildingSystem.Warehouse.Trans;
        }

        private void PickWellAsTarget()
        {
            // TODO TEMP
            var buildingSystem = GetSystem<BuildingSystem>();
            _blackboard[KeyTargetUnit] = buildingSystem.Well;
            _blackboard[KeyTargetTrans] = buildingSystem.Well.Trans;
        }

        private void PickWanderPos()
        {
            AIUtil.SetRandomPositionOnNavMesh(_blackboard, KeyTargetPos, Trans.position, Random.Range(_wanderDistanceRange.x, _wanderDistanceRange.y));
        }

        private void StopNavAgent()
        {
            if (NavAgent.enabled)
            {
                NavAgent.isStopped = true;
            }
        }

        private Node NavMoveToTargetWithAnim(string key, bool isCarrying = false)
        {
            return new Sequence(
                new Action(isCarrying? _cosmetic.PlayCarryAnim : _cosmetic.PlayWalkingAnim),
                new Parallel(Parallel.Policy.ONE, Parallel.Policy.ONE,
                    new NavMoveToTarget(NavAgent, key, 0.2f),
                    new Action(_cosmetic.UpdateFaceDirection)
                ),
                new Action(_cosmetic.PlayIdleAnim)
            );
        }

        #endregion
    }
}