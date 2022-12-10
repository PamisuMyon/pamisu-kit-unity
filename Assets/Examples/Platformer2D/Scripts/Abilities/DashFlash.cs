using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.Platformer2D.Abilities
{
    public class DashFlash : RecycleOnCondition
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void Play(Vector2 direction)
        {
            var animName = "Up";
            if (direction.y > 0)
                if (direction.x < 0)
                    animName = "Up_Left";
                else if (direction.x > 0)
                    animName = "Up_Right";
                else
                    animName = "Up";
            else if (direction.y < 0)
                if (direction.x < 0)
                    animName = "Down_Left";
                else if (direction.x > 0)
                    animName = "Down_Right";
                else
                    animName = "Down";
            else
                if (direction.x < 0)
                    animName = "Left";
                else if (direction.x > 0)
                    animName = "Right";
            anim.Play(animName);
        }
        
    }
}