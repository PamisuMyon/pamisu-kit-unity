using Pamisu.Inputs;
using UnityEngine;

namespace Pamisu.Gameplay.TopDown
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class TopDownPlayerController : MonoBehaviour
    {

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
        protected bool analogMovement = true;
        [SerializeField]
        protected float gravity = -15f;
        
        [Header("Ground Check")]
        [SerializeField][Tooltip("Read Only")]
        public bool IsGrounded;
        [SerializeField][Tooltip("Leave as -1 for auto detection")]
        protected float groundedRadius = -1f;
        [SerializeField][Tooltip("Leave as -1 for auto detection")]
        protected float groundedOffset = -1f;
        [SerializeField]
        protected LayerMask groundLayers;

        protected Vector3 basisForward;
        protected Vector3 basisRight;
        protected Vector3 velocity;

        protected CharacterController cc;
        public BasicPlayerInput Input { get; protected set; }

        public Vector3 Velocity => velocity;

        protected virtual void Start()
        {
            cc = GetComponent<CharacterController>();
            Input = GetComponent<BasicPlayerInput>();
            
            if (basisReferer != null)
                InitBasis(basisReferer.rotation);
            else
                InitBasis(transform.rotation);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedRadius == -1f)
                groundedRadius = cc.radius;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedOffset == -1f)
                groundedOffset = groundedRadius / 2f;
        }

        public void InitBasis(Quaternion basisRotation)
        {
            basisForward = basisRotation * Vector3.forward;
            basisRight = basisRotation * Vector3.right;
        }

        // protected virtual void Update()
        // {
        //     GroundedCheck();
        //     HandleMovement();
        // }

        public virtual void GroundedCheck()
        {
            var pos = transform.position;
            var checkPos = new Vector3(pos.x, pos.y + groundedOffset, pos.z);
            IsGrounded = Physics.CheckSphere(checkPos, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        public virtual void HandleMovement()
        {
            var targetVelocity = Vector3.zero;
            if (Input.Move != Vector2.zero)
            {
                var movement = Input.Move.x * basisRight + Input.Move.y * basisForward;
                if (!analogMovement)
                    movement = movement.normalized;

                var targetRotation = Quaternion.LookRotation(movement);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                targetVelocity = speed * movement;
            }

            if (!IsGrounded)
            {
                targetVelocity += Vector3.up * gravity;
            }

            velocity = Vector3.MoveTowards(velocity, targetVelocity, acc * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }
        
    }
}