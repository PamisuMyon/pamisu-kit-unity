using UnityEngine;

namespace Game.Common
{
    public static class AnimConst
    {
        public static readonly int IsRunning = Animator.StringToHash("IsRunning");
        public static readonly int Hurt = Animator.StringToHash("Hurt");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int MeleeAttack1 = Animator.StringToHash("MeleeAttack1");
        public static readonly int RangedAttack1 = Animator.StringToHash("RangedAttack1");
    }
}