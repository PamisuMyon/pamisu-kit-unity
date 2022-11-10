using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Game
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputBase))]
    public class TopDownPlayerControllerBase : MonoBehaviour
    {
        public enum MoveMode
        {
            Character,
            Wheeled,
            Caterpillar
        }


        [Header("Movement")]
        [SerializeField]
        protected float speed = 15f;
        [SerializeField]
        protected float acc = 80f;
        [SerializeField]
        protected float turnSpeed = 360f;
        [SerializeField]
        protected Transform basisReferer;
        [SerializeField]
        protected MoveMode moveMode = MoveMode.Character;
        [SerializeField]
        protected bool analogMovement = true;

        protected Vector3 basisForward;
        protected Vector3 basisRight;
        protected Vector3 velocity;

        protected CharacterController cc;
        protected PlayerInputBase input;

        protected virtual void Start()
        {
            cc = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputBase>();
            if (basisReferer != null)
                InitBasis(basisReferer.rotation);
            else
                InitBasis(transform.rotation);
        }

        public void InitBasis(Quaternion basisRotation)
        {
            basisForward = basisRotation * Vector3.forward;
            basisRight = basisRotation * Vector3.right;
        }

        protected virtual void Update()
        {
            HandleMovement();
        }

        protected virtual void HandleMovement()
        {
            var targetVelocity = Vector3.zero;
            if (input.Move != Vector2.zero)
            {
                var movement = input.Move.x * basisRight + input.Move.y * basisForward;
                if (!analogMovement)
                    movement = movement.normalized;

                var targetRotation = Quaternion.LookRotation(movement);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                if (moveMode == MoveMode.Character)
                {
                    targetVelocity = speed * movement;
                }
                else if (moveMode == MoveMode.Wheeled)
                {
                    targetVelocity = speed * transform.forward;
                }
                else if (moveMode == MoveMode.Caterpillar)
                {
                    if (transform.rotation.Approximately(targetRotation))
                        targetVelocity = speed * transform.forward;
                    else
                        targetVelocity = Vector3.zero;
                }
            }

            velocity = Vector3.MoveTowards(velocity, targetVelocity, acc * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }
    }
}