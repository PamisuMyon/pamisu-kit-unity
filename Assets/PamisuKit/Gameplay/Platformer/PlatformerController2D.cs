using UnityEngine;

namespace Pamisu.Gameplay.Platformer
{
    public class PlatformerController2D : Controller
    {

        [Header("Components")]
        [SerializeField]
        public SpriteRenderer MainSpriteRenderer;
        [SerializeField]
        public PlatformerMovement2D Movement;

        public Rigidbody2D Rigidbody => Movement.Rigidbody;
        
        protected virtual void Awake()
        {
            if (Movement == null)
                Movement = GetComponent<PlatformerMovement2D>();
        }
        
        protected virtual void Update()
        {
            Movement.GroundedCheck();
        }

        protected virtual void FixedUpdate()
        {
            Movement.HandleMovement();
        }

        public virtual void SetOrientation(bool faceLeft)
        {
            MainSpriteRenderer.flipX = faceLeft;
        }
        
    }
}