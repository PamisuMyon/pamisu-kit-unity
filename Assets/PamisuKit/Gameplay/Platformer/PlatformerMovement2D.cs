// ReSharper disable CompareOfFloatsByEqualityOperator

using System;
using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Gameplay.Platformer
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlatformerMovement2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        protected float speed = 8f;
        [SerializeField]
        protected float accelerationGround = 12f;
        [SerializeField]
        protected float accelerationAir = 8f;
        [SerializeField]
        protected float jumpForce = 12f;
        [SerializeField]
        protected float jumpHeldForce = .5f;
        [SerializeField]
        protected float jumpHeldDuration = .2f;
        [SerializeField]
        protected Vector2 verticalSpeedClamps;
        [SerializeField]
        protected bool useGravityOnGround = true;
        [SerializeField]
        protected bool velocitySnapToGround = true;
        [SerializeField]
        protected float slopeAngleLimit = 75f;

        [Header("Ground Check")]
        [SerializeField]
        protected LayerMask groundLayers;
        [SerializeField]
        [Tooltip("Detect radius. Leave as -1 for auto detection (Capsule size)")]
        protected float groundedRadius = -1f;
        [SerializeField]
        [Tooltip("Detect center Y offset. Leave as -1 for auto detection (Foot)")]
        protected float groundedYOffset = -1f;
        [SerializeField]
        [Tooltip("Cast distance. Leave as -1 for auto detection")]
        protected float groundedDistance = -1f;
        [SerializeField]
        protected float slopeCheckRadius = -1f;
        [SerializeField]
        protected float slopeCheckYOffset = -1f;
        [SerializeField]
        [Tooltip("Slope check X offset. Leave as -1 for auto detection")]
        protected float slopeCheckXOffset = -1f;
        [SerializeField]
        protected float slopeCheckDistance = -1f;


        [Header("Read Only")]
        public bool IsGrounded;
        public Vector2 SurfaceNormal = Vector2.up;
        public float SlopeAngle;
        public Vector2 TargetVelocity;

        public Rigidbody2D Rigidbody { get; protected set; }
        public CapsuleCollider2D Collider { get; protected set; }
        public bool CanJumpHeld { get; protected set; }
        public float OriginalGravityScale { get; set; }
        
        protected float jumpHeldCounter;
        protected ContactFilter2D groundedCheckFilter;
        protected readonly Collider2D[] groundedCheckCols = new Collider2D[16];
        protected readonly RaycastHit2D[] groundedCheckHits = new RaycastHit2D[1];

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void Start()
        {
            OriginalGravityScale = Rigidbody.gravityScale;

            if (groundedRadius == -1)
                groundedRadius = Collider.size.x / 2f;
            if (groundedYOffset == -1f)
                groundedYOffset = 0;
            if (groundedDistance == -1f)
                groundedDistance = groundedYOffset;
            if (slopeCheckRadius == -1f)
                slopeCheckRadius = groundedRadius / 2f;
            if (slopeCheckXOffset == -1f)
                slopeCheckXOffset = Collider.size.x / 2f;
            if (slopeCheckYOffset == -1f)
                slopeCheckYOffset = Collider.size.y / 2f;
            if (slopeCheckDistance == -1f)
                slopeCheckDistance = slopeCheckYOffset + groundedDistance + groundedRadius - slopeCheckRadius;

            groundedCheckFilter = new ContactFilter2D
            {
                useTriggers = false
            };
            groundedCheckFilter.SetLayerMask(groundLayers);
            groundedCheckFilter.SetDepth(float.NegativeInfinity, float.PositiveInfinity);
        }

        protected virtual void Update()
        {
            if (jumpHeldCounter > 0)
            {
                jumpHeldCounter -= Time.deltaTime;
                if (jumpHeldCounter <= 0)
                    CanJumpHeld = false;
            }
        }

        public virtual void GroundedCheck()
        {
            var contactNum = Rigidbody.GetContacts(groundedCheckFilter, groundedCheckCols);

            var pos = transform.position;
            var checkPos = new Vector2(pos.x, pos.y + groundedYOffset);
            var castNum = Physics2D.CircleCast(checkPos, groundedRadius, Vector2.down, groundedCheckFilter, groundedCheckHits, groundedDistance);

            Debug.DrawRay(checkPos, Vector3.down * (groundedRadius + groundedDistance), castNum != 0 ? Color.green : Color.red);

            if (!IsGrounded)
                IsGrounded = contactNum != 0 && castNum != 0;
            else
                IsGrounded = castNum != 0;

            SurfaceNormal = IsGrounded ? groundedCheckHits[0].normal : Vector2.up;

            // Slope check
            if (IsGrounded)
            {
                var xOffset = 0f;
                if (Rigidbody.velocity.x > 0.05f)
                    xOffset = slopeCheckXOffset;
                else if (Rigidbody.velocity.x < -0.05f)
                    xOffset = -slopeCheckXOffset;
                if (xOffset != 0)
                {
                    checkPos = new Vector2(pos.x + xOffset, pos.y + slopeCheckYOffset);
                    var slopeCastNum = Physics2D.CircleCast(checkPos, slopeCheckRadius, Vector2.down, groundedCheckFilter, groundedCheckHits, slopeCheckDistance);

                    Debug.DrawRay(checkPos, Vector2.down * (slopeCheckRadius + slopeCheckDistance), slopeCastNum != 0 ? Color.green : Color.red);

                    if (slopeCastNum != 0)
                        SurfaceNormal = groundedCheckHits[0].normal;
                }
            }

            SlopeAngle = Vector2.Angle(Vector2.up, SurfaceNormal);
            if (SlopeAngle > slopeAngleLimit)
            {
                IsGrounded = false;
            }

            ApplyGravity();
        }

        protected virtual void ApplyGravity()
        {
            if (IsGrounded &&
                (!useGravityOnGround
                 || (SlopeAngle > 5f && SlopeAngle <= slopeAngleLimit)))
                Rigidbody.gravityScale = 0;
            else
                Rigidbody.gravityScale = OriginalGravityScale;
        }

        public virtual void Move(float inputX)
        {
            TargetVelocity.x = inputX * speed;
            TargetVelocity.y = Rigidbody.velocity.y;

            // Ground snap
            if (IsGrounded
                && velocitySnapToGround
                && !CanJumpHeld)
            {
                // Parallel to the right direction of the current surface
                var dir = Vector3.Cross(SurfaceNormal, Vector3.forward).normalized;
                TargetVelocity = inputX * speed * dir;
            }
        }
        
        public virtual bool Jump()
        {
            if (IsGrounded && !CanJumpHeld)
            {
                // TargetVelocity.y = 0; // TODO if not on moving platformer
                // TargetVelocity.y += jumpForce;
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
                Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
                jumpHeldCounter = jumpHeldDuration;
                CanJumpHeld = true;
                IsGrounded = false;
                return true;
            }

            return false;
        }

        public virtual void JumpHeld()
        {
            if (!CanJumpHeld) return;
            // TargetVelocity.y = Rigidbody.velocity.y + jumpHeldForce;
            Rigidbody.AddForce(Vector2.up * jumpHeldForce, ForceMode2D.Impulse);
        }

        public virtual void ApplyVelocity(bool useAcceleration = true)
        {
            Vector2 velocity; 
            if (useAcceleration)
            {
                var acc = IsGrounded ? accelerationGround : accelerationAir;
                velocity.x = Rigidbody.velocity.x.MoveTowards(TargetVelocity.x, acc);
                velocity.y = TargetVelocity.y;
            }
            else
                velocity = TargetVelocity;
            if (verticalSpeedClamps != Vector2.zero)
                velocity.y = Mathf.Clamp(velocity.y, verticalSpeedClamps.x, verticalSpeedClamps.y);
            
            Rigidbody.velocity = velocity;
        }

    }
}