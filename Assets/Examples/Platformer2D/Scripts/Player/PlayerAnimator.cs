using Pamisu.Gameplay.Platformer;
using UnityEngine;

namespace Pamisu.Platformer2D.Player
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField]
        private PlatformerMovement2D movement;
        
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            if (movement == null)
                movement = GetComponentInParent<PlatformerMovement2D>();
        }

        private void Update()
        {
            var velocity = movement.TargetVelocity;
            anim.SetFloat(AnimID.GroundSpeed, Mathf.Abs(velocity.x));
            anim.SetFloat(AnimID.AirborneSpeed, velocity.y);
            anim.SetBool(AnimID.IsGrounded, movement.IsGrounded);
        }
    }
}