using System;
using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Gameplay.Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public abstract class PlatformerController2D : Controller
    {
        [Header("Movement")]
        [SerializeField]
        protected float walkSpeed;
        [SerializeField]
        protected float runSpeed;
        [SerializeField]
        protected float accelerationGround;
        [SerializeField]
        protected float accelerationAir;
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float jumpHeldForce;
        [SerializeField]
        protected float jumpHeldDuration;
        [SerializeField]
        protected Vector2 verticalSpeedClamps;
        [SerializeField]
        protected bool useGravityOnGround;
        [SerializeField]
        protected bool velocitySnapToGround = true;


        [Header("Ground Check")]
        [SerializeField]
        [Tooltip("Read Only")]
        public bool IsGrounded;
        [SerializeField]
        [Tooltip("Detect radius. Leave as -1 for auto detection (Capsule size)")]
        protected float groundedRadius = -1f;
        [SerializeField]
        [Tooltip("Detect center X offset. Leave as -1 for auto detection")]
        protected float groundedXOffset = -1f;
        [SerializeField]
        [Tooltip("Detect center Y offset. Leave as -1 for auto detection")]
        protected float groundedYOffset = -1f;
        [SerializeField]
        [Tooltip("Cast distance. Leave as -1 for auto detection")]
        protected float groundedDistance = -1f;
        [SerializeField]
        protected LayerMask groundLayers;
        

        [Header("Components")]
        [SerializeField]
        public SpriteRenderer mainSpriteRenderer;

        public Rigidbody2D Rigidbody { get; protected set; }
        public CapsuleCollider2D Collider { get; protected set; }
        public PlayerInputBase Input { get; protected set; }

        protected float currentSpeed;
        protected float gravityScale;
        protected bool isJumping;
        protected float jumpCounter;
        protected ContactFilter2D groundedCheckFilter;
        protected readonly Collider2D[] groundedCheckCols = new Collider2D[16];
        protected readonly RaycastHit2D[] groundedCheckHits = new RaycastHit2D[1];
        public Vector2 SurfaceNormal { get; protected set; }


        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CapsuleCollider2D>();
            Input = GetComponent<PlayerInputBase>();
            if (mainSpriteRenderer == null)
                mainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void Start()
        {
            currentSpeed = runSpeed;
            gravityScale = Rigidbody.gravityScale;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedRadius == -1)
                groundedRadius = Collider.size.x / 2f;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedXOffset == -1f)
                groundedXOffset = groundedRadius;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedYOffset == -1f)
                groundedYOffset = 0;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedDistance == -1f)
                groundedDistance = groundedYOffset;

            groundedCheckFilter = new ContactFilter2D
            {
                useTriggers = false
            };
            groundedCheckFilter.SetLayerMask(groundLayers);
            groundedCheckFilter.SetDepth(float.NegativeInfinity, float.PositiveInfinity);
        }

        public virtual void GroundedCheck()
        {
            var contactNum = Rigidbody.GetContacts(groundedCheckFilter, groundedCheckCols);

            if (contactNum != 0)
            {
                var pos = transform.position;
                var checkPos = new Vector2(pos.x, pos.y + groundedYOffset);
                var castNum = Physics2D.CircleCast(checkPos, groundedRadius, Vector2.down, groundedCheckFilter, groundedCheckHits, groundedDistance);
            
                Debug.DrawRay(checkPos, Vector3.down * (groundedRadius + groundedDistance), castNum != 0 ? Color.green : Color.red);
                
                IsGrounded = castNum != 0;
                SurfaceNormal = IsGrounded ? groundedCheckHits[0].normal : Vector2.up;

                if (IsGrounded)
                {
                    var xOffset = 0f;
                    if (Input.Move.x > 0)
                        xOffset = groundedXOffset;
                    else if (Input.Move.x < 0)
                        xOffset = -groundedXOffset;
                    if (xOffset != 0)
                    {
                        checkPos.x += xOffset;
                        castNum = Physics2D.CircleCast(checkPos, groundedRadius, Vector2.down, groundedCheckFilter, groundedCheckHits, groundedDistance);
                    
                        Debug.DrawRay(checkPos, Vector3.down * (groundedRadius + groundedDistance), castNum != 0 ? Color.green : Color.red);

                        if (castNum != 0)
                            SurfaceNormal = groundedCheckHits[0].normal;
                    }
                }
            }
            else
            {
                IsGrounded = false;
            }
        }

        public virtual void HandleMovement()
        {
            if (!useGravityOnGround)
            {
                Rigidbody.gravityScale = IsGrounded ? 0 : gravityScale;
            }

            var targetVelocity = new Vector2(Input.Move.x * runSpeed, Rigidbody.velocity.y);
            
            // Jump
            if (Input.Jump)
            {
                if (IsGrounded && !isJumping)
                {
                    jumpCounter = Time.time + jumpHeldDuration;
                    targetVelocity.y += jumpForce;
                    isJumping = true;
                }

                Input.Jump = false;
            }
            if (isJumping)
            {
                if (Input.JumpHeld)
                    targetVelocity.y += jumpHeldForce;

                if (jumpCounter < Time.time)
                    isJumping = false;
            }

            // Ground snap
            if (IsGrounded 
                && velocitySnapToGround 
                && !isJumping)
            {
                // Parallel to the right direction of the current surface
                var dir = Vector3.Cross(SurfaceNormal, Vector3.forward).normalized;
                targetVelocity = Input.Move.x * runSpeed * dir;
            }

            // Apply velocity
            var acc = IsGrounded ? accelerationGround : accelerationAir;
            var velocity = new Vector2
            {
                x = Rigidbody.velocity.x.MoveTowards(targetVelocity.x, acc),
                y = targetVelocity.y
            };
            if (verticalSpeedClamps != Vector2.zero)
                velocity.y = Mathf.Clamp(velocity.y, verticalSpeedClamps.x, verticalSpeedClamps.y);
            Rigidbody.velocity = velocity;
        }

        public virtual void HandleOrientation()
        {
            if (Input.Move.x > 0)
                mainSpriteRenderer.flipX = false;
            else if (Input.Move.x < 0)
                mainSpriteRenderer.flipX = true;
        }
        
    }
}