using Pamisu.Commons.FSM;
using Pamisu.Commons.Pool;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Pamisu.TopDownShooter.Player.States
{

    public class Blackboard
    {
        public float AimDelayCounter;
    }
    
    public abstract class PlayerState : State
    {
        protected PlayerController p;

        public PlayerState(PlayerController playerController)
        {
            p = playerController;
        }
        
        public override void OnProcess()
        {
            base.OnProcess();
            p.GroundedCheck();
        }
    }

    public class NormalState : PlayerState
    {
        public NormalState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.IsAiming = false;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            p.HandleMovement();
            p.RotateTowardsMovement();
            p.HandleCannon();

            if (p.Input.Fire1)
            {
                Machine.ChangeState<PreAimState>();
            }
        }
        
    }
    
    public class PreAimState : PlayerState
    {

        private Vector3 targetDir;
        
        public PreAimState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (!p.GetAimDirection(out targetDir))
            {
                Machine.ChangeState<NormalState>();
                return;
            }
            p.IsAiming = true;
            p.Blackboard.AimDelayCounter = p.AimAnimationDelay;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            p.HandleMovement();
            p.HandleCannon();
            p.Blackboard.AimDelayCounter -= Time.deltaTime;
            
            if (!p.Input.Fire1)
            {
                Machine.ChangeState<NormalState>();
                return;
            }

            if (p.RotateTowards(targetDir, 1440f))
            {
                Machine.ChangeState<ShootState>();
            }
        }
    }

    public class ShootState : PlayerState
    {

        private float fireCounter;
        
        public ShootState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            fireCounter = 0;
        }

        public override void OnProcess()
        {
            base.OnProcess();
            p.HandleMovement();
            p.HandleCannon();
            p.RotateTowardsAim();

            if (!p.Input.Fire1)
            {
                Machine.ChangeState<NormalState>();
                return;
            }

            if (p.Blackboard.AimDelayCounter > 0)
            {
                p.Blackboard.AimDelayCounter -= Time.deltaTime;
                return;
            }

            if (fireCounter > 0)
            {
                fireCounter -= Time.deltaTime;
                return;
            }

            var go = GameObjectPooler.Spawn(p.ProjectilePrefab);
            go.GetComponent<Projectile>().Spawn(p.FirePoint.position, p.transform.rotation, p.gameObject.layer);
            fireCounter = p.FireInterval;
        }
        
    }
    
    public class DeathState : PlayerState
    {
        public DeathState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            p.gameObject.layer = LayerMask.NameToLayer("Void");
            // TODO Game Over
        }
        
    }
}