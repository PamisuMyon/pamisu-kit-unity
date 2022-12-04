using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public static class AnimID
    {
        public static readonly int GroundSpeedX = Animator.StringToHash("GroundSpeedX");
        public static readonly int GroundSpeedZ = Animator.StringToHash("GroundSpeedZ");
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");
        
        public static readonly int GroundSpeed = Animator.StringToHash("GroundSpeed");
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int Shoot = Animator.StringToHash("Shoot");
    }
}