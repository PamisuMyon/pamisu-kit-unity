using UnityEngine;

namespace Pamisu.Gameplay.Platformer
{
    public class PlatformerPlayerController2D : Controller
    {

        [Header("Components")]
        [SerializeField]
        public PlatformerMovement2D Movement;

        public BasicPlayerInput Input { get; protected set; }
        
        protected virtual void Awake()
        {
            if (Movement == null)
                Movement = GetComponent<PlatformerMovement2D>();
            Input = GetComponent<BasicPlayerInput>();
        }
        
        protected virtual void Update()
        {
            Movement.GroundedCheck();
        }

        protected virtual void FixedUpdate()
        {
            if (Input.Jump)
            {
                Movement.Jump();
                Input.Jump = false;
            }

            if (Input.JumpHeld && Movement.CanJumpHeld)
            {
                Movement.JumpHeld();
            }

            Movement.Move(Input.Move.x);
            Movement.ApplyVelocity();
        }

    }
}