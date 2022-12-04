using DG.Tweening;
using Pamisu.Commons;
using Pamisu.Commons.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Pamisu.TopDownShooter.Enemies.States
{

    public class Blackboard
    {
        public Vector3 MoveTarget;
        public ActorAttributes TrackTarget;
        public float ActionBlockCounter;
        public EnemyAction CurrentAction;

        public bool IsActionBlocking => ActionBlockCounter > 0;
    }
    
    public class EnemyState : State
    {
        protected EnemyController p;

        public EnemyState(EnemyController enemyController)
        {
            p = enemyController;
        }
    }

    public class NoneState : EnemyState
    {
        public NoneState(EnemyController enemyController) : base(enemyController)
        {
        }
    }
    
    public class IdleState : EnemyState
    {

        private float idleCounter;
        
        public IdleState(EnemyController enemyController) : base(enemyController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            idleCounter = RandomUtil.RandomNum(p.IdleTime, p.IdleTimeRandomness);
            p.Agent.isStopped = true;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            if (idleCounter > 0)
            {
                idleCounter -= Time.deltaTime;
                return;
            }
            // Random patrol destination
            UnityUtil.RandomPointOnNavMesh(p.transform.position, p.PatrolDistance, out p.Blackboard.MoveTarget);
            Machine.ChangeState<MoveState>();
        }
    }
    
    public class MoveState : EnemyState
    {
        public MoveState(EnemyController enemyController) : base(enemyController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.Agent.destination = p.Blackboard.MoveTarget;
            p.Agent.stoppingDistance = 0f;
            p.Agent.speed = p.WalkSpeed;
            p.Agent.isStopped = false;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            if (!p.Agent.pathPending 
                && p.Agent.remainingDistance <= p.Agent.stoppingDistance)
            {
                if (p.Blackboard.TrackTarget != null
                    && !p.Blackboard.TrackTarget.IsDied)
                    Machine.ChangeState<TrackState>();
                else
                    Machine.ChangeState<IdleState>();
            }
        }
        
    }
    
    public class TrackState : EnemyState
    {
        
        private float trackCounter;
        
        public TrackState(EnemyController enemyController) : base(enemyController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.Agent.speed = p.RunSpeed;
            p.Agent.stoppingDistance = p.GetTargetStoppingDistance();
            p.Agent.isStopped = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            p.Agent.isStopped = true;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            var dir = p.Blackboard.TrackTarget.transform.position - p.transform.position;
            if (p.Agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                p.RotateTowards(dir, p.Agent.angularSpeed);
            }
            
            if (p.GetAvailableAction(dir.sqrMagnitude, out p.Blackboard.CurrentAction))
            {
                Machine.ChangeState<ActionState>();
                return;
            }
            
            if (trackCounter > 0)
            {
                trackCounter -= Time.deltaTime;
                return;
            }
            p.Agent.destination = p.Blackboard.TrackTarget.transform.position;
            trackCounter = p.TrackInterval;

        }
        
    }
    
    public class ActionState : EnemyState
    {

        public ActionState(EnemyController enemyController) : base(enemyController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.Blackboard.CurrentAction.Perform(OnActionPerformed);
        }

        private void OnActionPerformed()
        {
            UnityUtil.RandomPointOnNavMesh(p.transform.position, p.HitAndRunDistance, out p.Blackboard.MoveTarget);
            Machine.ChangeState<MoveState>();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            p.Blackboard.CurrentAction.Stop();
        }
    }
    
    public class DeathState : EnemyState
    {
        public DeathState(EnemyController enemyController) : base(enemyController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.gameObject.layer = LayerMask.NameToLayer("Void");
            var hurtBox = p.transform.Find("HurtBox");
            if (hurtBox)
                hurtBox.gameObject.layer = p.gameObject.layer;
            
            p.Agent.isStopped = true;
            p.Animator.SetTrigger(AnimID.Die);
            p.DetectArea.gameObject.SetActive(false);
            p.StartCoroutine(UnityUtil.Delay(2.5f, () =>
            {
                var targetY = p.transform.position.y - 2.5f;
                p.transform.DOMoveY(targetY, 2f)
                    .OnComplete(() =>
                    {
                        Object.Destroy(p.gameObject);
                    });
            }));
        }
        
    }
}