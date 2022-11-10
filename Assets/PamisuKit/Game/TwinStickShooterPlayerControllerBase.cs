using UnityEngine;

namespace Pamisu.Game
{
    public class TwinStickShooterPlayerControllerBase : TopDownPlayerControllerBase
    {
        [Header("Shooter")]
        [Header("Movement")]
        protected float aimSpeed = 5f;
        [SerializeField]
        protected float aimAcc = 50f;
        [SerializeField]
        protected float aimTurnSpeed = 180f;

        protected new MoveMode moveMode = MoveMode.Character;
        
        public bool IsAiming { get; set; }

        protected override void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        protected override void HandleMovement()
        {
            var targetVelocity = Vector3.zero;
            if (input.Move != Vector2.zero)
            {
                var movement = input.Move.x * basisRight + input.Move.y * basisForward;
                if (!analogMovement)
                    movement = movement.normalized;

                var targetRotation = Quaternion.LookRotation(movement);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, 
                        (IsAiming? aimTurnSpeed : turnSpeed) * Time.deltaTime);

                targetVelocity = (IsAiming? aimSpeed : speed) * movement;
            }

            velocity = Vector3.MoveTowards(velocity, targetVelocity, (IsAiming? aimAcc : acc) * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }

        protected virtual void HandleRotation()
        {
            
        }
        
    }
}