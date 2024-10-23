using NPBehave;
using UnityEngine;
using UnityEngine.AI;

namespace Game.AI.Actions
{
    public class NavMoveToTarget : Task
    {

        private NavMeshAgent _agent;
        private string _targetKey;
        private float _tolerance;
        private float _checkIsReachedFrequency;
        private float _updateDestinationFrequency;

        private float _lastDistance;
        
        public NavMoveToTarget(
            NavMeshAgent agent, 
            string targetKey, 
            float tolerance = 1f,
            float checkIsReachedFrequency = .1f, 
            float updateDestinationFrequency = .3f) : base("NavMoveToTarget")
        {
            _agent = agent;
            _targetKey = targetKey;
            _tolerance = tolerance;
            _checkIsReachedFrequency = checkIsReachedFrequency;
            _updateDestinationFrequency = updateDestinationFrequency;
        }

        protected override void DoStart()
        {
            base.DoStart();

            _lastDistance = float.PositiveInfinity;
            
            Clock.AddTimer(_checkIsReachedFrequency, 0f, -1, CheckIsReachedTarget);
            if (_updateDestinationFrequency >= 0)
                Clock.AddTimer(_updateDestinationFrequency, 0f, -1, UpdateDestination);
            
            UpdateDestination();
        }
        
        private void UpdateDestination()
        {
            var target = Blackboard.Get(_targetKey);
            if (target == null)
            {
                StopAndCleanUp(false);
                return;
            }

            if (target is Transform targetTrans)
            {
                _agent.destination = targetTrans.position;
                _agent.isStopped = false;
            }
            else if (target is Vector3 targetPos)
            {
                _agent.destination = targetPos;
                _agent.isStopped = false;
            }
            else if (target is GameObject targetGo)
            {
                _agent.destination = targetGo.transform.position;
                _agent.isStopped = false;
            }
            else
            {
                Debug.LogWarning($"{GetType().Name}: Unsupported target type {target.GetType()} of blackboard key {_targetKey}");
                StopAndCleanUp(false);
            }
        }

        private void CheckIsReachedTarget()
        {
            if (_lastDistance < _tolerance)
            {
                StopAndCleanUp(true);
                return;
            }

            if (_agent.pathPending)
                _lastDistance = float.PositiveInfinity;
            else
                _lastDistance = _agent.remainingDistance;
        }

        protected override void DoStop()
        {
            StopAndCleanUp(false);
        }

        private void StopAndCleanUp(bool result)
        {
            _agent.isStopped = true;
            Clock.RemoveTimer(CheckIsReachedTarget);
            Clock.RemoveTimer(UpdateDestination);
            Stopped(result);
        }
        
    }
}