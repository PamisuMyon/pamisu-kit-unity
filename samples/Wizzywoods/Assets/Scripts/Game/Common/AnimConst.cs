using UnityEngine;

namespace Game.Common
{
    public static class AnimConst
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int Hurt = Animator.StringToHash("Hurt");
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int CastShoot = Animator.StringToHash("CastShoot");
        public static readonly int MeleeAttack1 = Animator.StringToHash("MeleeAttack1");

    }
}