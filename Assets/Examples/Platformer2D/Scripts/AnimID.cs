using UnityEngine;

namespace Pamisu.Platformer2D
{
    public static class AnimID
    {
        public static readonly int GroundSpeed = Animator.StringToHash("GroundSpeed");
        public static readonly int AirborneSpeed = Animator.StringToHash("AirborneSpeed");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsDucking = Animator.StringToHash("IsDucking");
        public static readonly int IsSliding = Animator.StringToHash("IsSliding");
    }
}