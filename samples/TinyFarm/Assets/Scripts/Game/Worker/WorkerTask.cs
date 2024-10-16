using System;
using Game.Farm;
using Game.Framework;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Worker.Models
{
    public enum WorkerTaskType
    {
        None,
        Watering,
        Harvesting,
        Construction,
    }

    public enum WorkerTaskState
    {
        Pending,
        InProgress,
        Completed,
    }

    public class WorkerTask
    {
        public string TargetId;
        public WorkerTaskType Type;
        public WorkerTaskState State;

        private Unit _target;

        public WorkerTask(string targetId, WorkerTaskType type, WorkerTaskState state = WorkerTaskState.Pending)
        {
            TargetId = targetId;
            Type = type;
            State = state;
        }

        public WorkerTask(Unit target, WorkerTaskType type, WorkerTaskState state = WorkerTaskState.Pending)
        {
            TargetId = target.Id;
            _target = target;
            Type = type;
            State = state;
        }

        public Unit GetTarget(MonoEntity entity)
        {
            if (_target != null)
                return _target;
            if (string.IsNullOrEmpty(TargetId))
                return null;
            if (Type == WorkerTaskType.Watering || Type == WorkerTaskType.Harvesting)
            {
                var patchSystem = entity.GetSystem<PatchSystem>();
                var plot = patchSystem.GetPlotById(TargetId);
                Debug.Assert(plot != null, $"Plot with Id {TargetId} not found.");
                _target = plot;
                return _target;
            }
            return _target;
        }
        
    }
}