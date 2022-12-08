using UnityEngine;

namespace Pamisu.Platformer2D.Player
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField]
        private PlayerController controller;
        
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            if (controller == null)
                controller = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            var velocity = controller.Rigidbody.velocity;
            anim.SetFloat(AnimID.GroundSpeed, Mathf.Abs(velocity.x));
            anim.SetFloat(AnimID.AirborneSpeed, velocity.y);
            anim.SetBool(AnimID.IsOnGround, controller.Movement.IsGrounded);
        }
    }
}