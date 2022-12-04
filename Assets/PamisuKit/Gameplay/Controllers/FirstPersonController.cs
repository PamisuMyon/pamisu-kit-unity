using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Gameplay.Controllers
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputBase))]
    public class FirstPersonController : Controller
    {
        [Header("Base")]
        [SerializeField]
        protected Transform armsTransform;
        
        [Header("Ground Check")]
        [SerializeField]
        protected LayerMask groundLayer;
        [SerializeField]
        protected float groundCheckDistance = 0.1f;

        [Header("Movement")]
        [SerializeField]
        protected float groundMaxSpeed = 10f;
        [SerializeField]
        protected float sprintSpeedModifier = 1.5f;
        [SerializeField]
        protected float sprintStaminaCost = 0f;
        [SerializeField]
        protected float speedLerpGround = 15f;
        [SerializeField]
        protected float airMaxSpeed = 7.6f;
        [SerializeField]
        protected float accelerationAir = 20f;
        [SerializeField]
        protected float jumpForce = 9f;
        [SerializeField]
        protected float gravity = 20f;
        [SerializeField]
        protected float footstepInterval = .4f;
        [SerializeField]
        protected AudioSource footstepAudio;

        [Header("Look")]
        [SerializeField]
        protected float rotationSpeed = 200f;
        [SerializeField]
        protected float minVerticalAngle = -89f;
        [SerializeField]
        protected float maxVerticalAngle = 89f;

        protected CharacterController cc;
        protected PlayerInputBase input;
        protected bool isGrounded;
        protected Vector3 groundNormal;
        protected float armsVerticalAngle = 0f;
        protected Vector3 currentVelocity;
        protected float footstepCounter;

        protected virtual void Start()
        {
            cc = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputBase>();

            if (armsTransform == null)
                armsTransform = transform.Find("Arms");
        }

        protected virtual void Update()
        {
            HandleRotation();
            GroundCheck();
        }

        protected virtual void FixedUpdate()
        {
            HandleMovement();
        }

        void HandleRotation()
        {
            // Horizontal
            var rotH = new Vector3(0f, input.Look.x * rotationSpeed, 0f);
            transform.Rotate(rotH, Space.Self);

            // Vertical
            armsVerticalAngle += input.Look.y * rotationSpeed;
            armsVerticalAngle = Mathf.Clamp(armsVerticalAngle, minVerticalAngle, maxVerticalAngle);

            armsTransform.transform.localEulerAngles = new Vector3(armsVerticalAngle, 0, 0);
        }

        void GroundCheck()
        {
            isGrounded = false;
            groundNormal = Vector3.up;

            // Use capsule of CharacterController
            var innerHeight = (cc.height - 2 * cc.radius) / 2;
            var point1 = transform.position + Vector3.up * innerHeight;
            var point2 = transform.position + Vector3.down * innerHeight;
            var b = Physics.CapsuleCast(point1, point2, cc.radius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);

            if (b)
            {
                groundNormal = hit.normal;
                if (Vector3.Dot(hit.normal, transform.up) > 0f && IsSlope(groundNormal))
                {
                    isGrounded = true;
                    // // Snap to ground
                    // cc.Move(Vector3.down * hit.distance);
                }
            }
        }

        bool IsSlope(Vector3 groundNormal)
        {
            var angle = Vector3.Angle(transform.up, groundNormal);
            return angle <= cc.slopeLimit;
        }

        void HandleMovement()
        {
            Vector3 movement = transform.TransformVector(input.Move);
            float speedModifier = input.Sprint ? sprintSpeedModifier : 1f;

            if (isGrounded)
            {
                // Ground Move
                var directionSide = Vector3.Cross(movement, transform.up);
                var directionSlope = Vector3.Cross(groundNormal, directionSide);
                var targetVelocity = directionSlope.normalized * (groundMaxSpeed * speedModifier);

                currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, speedLerpGround * Time.deltaTime);

                // Jump
                if (input.Jump)
                {
                    currentVelocity += Vector3.up * jumpForce;
                    input.Jump = false;
                    isGrounded = false;
                }
                
                // Footstep
      
                if (Time.time > footstepCounter
                    && movement != Vector3.zero)
                {
                    footstepAudio.PlayRandomPitch();
                    footstepCounter = Time.time + footstepInterval;
                }
            }
            else
            {
                // Air move
                currentVelocity += movement * (accelerationAir * Time.deltaTime);
                // Clamp horizontal speed in air
                var verticalVelocity = currentVelocity.y;
                var horizontalVelocity = Vector3.ProjectOnPlane(currentVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, airMaxSpeed * speedModifier);
                currentVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

                currentVelocity += Vector3.down * (gravity * Time.deltaTime);
            }

            cc.Move(currentVelocity * Time.deltaTime);
        }

    }
}