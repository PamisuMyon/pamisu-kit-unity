using UnityEngine;

namespace Pamisu.Game
{
    public class TwinStickShooterPlayerControllerBase : TopDownPlayerControllerBase
    {
        [Space]
        [Header("Twin Stick Shooter")]
        [Header("Movement")]
        [SerializeField]
        protected float aimSpeed = 5f;
        [SerializeField]
        protected float aimAcc = 50f;
        [SerializeField]
        protected float aimTurnSpeed = 180f;

        public bool IsAiming; // { get; set; }

        protected override void Update()
        {
            HandleMovement();
        }

        protected override void HandleMovement()
        {
            var movement = Vector3.zero;
            var targetVelocity = Vector3.zero;
            Quaternion targetRotation = default;
            var shouldApplyRotation = false;
                
            if (input.Move != Vector2.zero)
            {
                movement = input.Move.x * basisRight + input.Move.y * basisForward;
                if (!analogMovement)
                    movement = movement.normalized;
            }
            
            if (IsAiming)
            {
                targetVelocity = aimSpeed * movement;

                if (input.CurrentDevice == null || input.CurrentDevice.path.Contains("Mouse") 
                                                || input.CurrentDevice.path.Contains("Keyboard"))
                {
                    var ray = Camera.main.ScreenPointToRay(input.MousePosition);
                    var isHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity);
                    if (isHit)
                    {
                        Debug.DrawRay(hit.point, Vector3.up, Color.cyan);
                        var lookDir = hit.point - transform.position;
                        targetRotation = Quaternion.LookRotation(lookDir);
                        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, aimTurnSpeed * Time.deltaTime);
                        shouldApplyRotation = true;
                    }
                }
                else
                {
                    var lookDir = input.Move.x * basisRight + input.Move.y * basisForward;
                    Debug.DrawRay(transform.position, lookDir, Color.cyan);
                    targetRotation = Quaternion.LookRotation(lookDir);
                    targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, aimTurnSpeed * Time.deltaTime);
                    shouldApplyRotation = true;
                }
            }
            else if (movement != Vector3.zero)
            {
                targetVelocity = speed * movement;
                
                targetRotation = Quaternion.LookRotation(movement);
                targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                shouldApplyRotation = true;
            }
            
            if (shouldApplyRotation)
                transform.rotation = targetRotation;

            velocity = Vector3.MoveTowards(velocity, targetVelocity, (IsAiming ? aimAcc : acc) * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }

    }
}