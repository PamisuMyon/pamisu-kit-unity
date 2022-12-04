using System;
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
        protected float accGround;
        [SerializeField]
        protected float accAir;
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float jumpHeldForce;
        [SerializeField]
        protected float jumpHeldDuration;
        [SerializeField]
        protected Vector2 verticalSpeedClamps;

        
        [Header("Ground Check")]
        [SerializeField][Tooltip("Read Only")]
        public bool IsGrounded;
        [SerializeField][Tooltip("Leave as -1 for auto detection")]
        protected float groundedRadius = -1f;
        [SerializeField][Tooltip("Leave as -1 for auto detection")]
        protected float groundedOffset = -1f;
        [SerializeField]
        protected LayerMask groundLayers;

        protected ContactFilter2D groundedCheckFilter;
        protected readonly Collider2D[] groundedCheckCols = new Collider2D[1];
        
        public Rigidbody2D Rigidbody { get; protected set; }
        public CapsuleCollider2D Collider { get; protected set; }

        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CapsuleCollider2D>();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedRadius == -1)
                groundedRadius = Collider.size.x / 2f;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (groundedOffset == -1f)
                groundedOffset = groundedRadius / 2f;

            groundedCheckFilter = new ContactFilter2D
            {
                useTriggers = false
            };
            groundedCheckFilter.SetLayerMask(groundLayers);
            groundedCheckFilter.SetDepth(float.NegativeInfinity, float.PositiveInfinity);
        }
        
        public virtual void GroundedCheck()
        {
            var pos = transform.position;
            var checkPos = new Vector2(pos.x, pos.y + groundedOffset);
            var num = Physics2D.OverlapCircle(checkPos, groundedRadius, groundedCheckFilter, groundedCheckCols);
            IsGrounded = num != 0;
        }

        public virtual void HandleMovement()
        {
            
        }
        
    }
}