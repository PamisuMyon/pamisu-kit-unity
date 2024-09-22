using Game.Common;
using NPBehave;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Combat.AI.Actions
{
    public class AnimByNavAgent : Task
    {

        private Animator _anim;
        private NavMeshAgent _agent;
        private float _updateFrequency;
        
        public AnimByNavAgent(Animator animator, NavMeshAgent agent, float updateFrequency = .1f) : base("AnimByNavAgent")
        {
            _anim = animator;
            _agent = agent;
            _updateFrequency = updateFrequency;
        }

        protected override void DoStart()
        {
            base.DoStart();
            Clock.AddTimer(_updateFrequency, 0f, -1, OnUpdate);
        }
        
        private void OnUpdate()
        {
            _anim.SetBool(AnimConst.IsMoving, !_agent.isStopped);
            // TODO blend tree
        }

        protected override void DoStop()
        {
            base.DoStop();
            StopAndCleanUp();
        }

        private void StopAndCleanUp()
        {
            _anim.SetBool(AnimConst.IsMoving, false);
            Stopped(true);
        }
        
    }
}