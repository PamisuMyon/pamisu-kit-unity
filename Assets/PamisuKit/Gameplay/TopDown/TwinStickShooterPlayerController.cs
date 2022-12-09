using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Gameplay.TopDown
{
    public abstract class TwinStickShooterPlayerController : TopDownPlayerController
    {
        [Space]
        [Header("Twin Stick Shooter")]
        [Header("Aiming")]
        [SerializeField]
        protected float aimSpeed = 5f;
        [SerializeField]
        protected float aimAcc = 50f;
        [SerializeField]
        protected float aimTurnSpeed = 180f;
        [SerializeField]
        protected LayerMask mouseCastLayers;


        protected Vector3 movement;
        
        public bool IsAiming { get; set; }
        
        public override void HandleMovement()
        {
            if (Input.Move != Vector2.zero)
            {
                movement = Input.Move.x * basisRight + Input.Move.y * basisForward;
                if (!analogMovement)
                    movement = movement.normalized;
            }
            else
                movement = Vector3.zero;
            
            var targetVelocity = (IsAiming? aimSpeed : speed) * movement;
            if (!IsGrounded)
                targetVelocity = Vector3.up * gravity;

            velocity = Vector3.MoveTowards(velocity, targetVelocity, (IsAiming ? aimAcc : acc) * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }

        public virtual bool RotateTowards(Vector3 direction, float rotateSpeed = 0)
        {
            if (rotateSpeed == 0) rotateSpeed = turnSpeed;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            return transform.rotation.Approximately(targetRotation, 0.005f);
        }

        public virtual bool RotateTowardsMovement()
        {
            if (movement != Vector3.zero)
                return RotateTowards(movement);
            return false;
        }

        public bool GetAimDirection(out Vector3 direction)
        {
            direction = default;
            if (Input.CurrentDevice == null || Input.CurrentDevice.path.Contains("Mouse") 
                                            || Input.CurrentDevice.path.Contains("Keyboard"))
            {
                var ray = Camera.main.ScreenPointToRay(Input.MousePosition);
                var isHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity, mouseCastLayers);
                if (isHit)
                {
                    Debug.DrawRay(hit.point, Vector3.up, Color.cyan);
                    direction = hit.point - transform.position;
                    direction.y = 0;
                    return true;
                }
            }
            else
            {
                direction = Input.Move.x * basisRight + Input.Move.y * basisForward;
                Debug.DrawRay(transform.position, direction, Color.cyan);
                return true;
            }
            return false;
        }
        
        public virtual bool RotateTowardsAim()
        {
            if (GetAimDirection(out var dir))
                return RotateTowards(dir, aimTurnSpeed);
            return false;
        }
        
    }
}