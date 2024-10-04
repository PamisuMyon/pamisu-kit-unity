using PamisuKit.Common.Util;
using NPBehave;
using UnityEngine;

namespace Game.Combat.AI
{
    public static class AIUtil
    {

        public static void SetRandomPositionOnNavMesh(
            this Blackboard blackboard, 
            string key,
            Vector3 center, 
            float radius)
        {
            if (RandomUtil.RandomPositionOnNavMesh(center, radius, out var result))
            {
                blackboard.Set(key, result);
            }
        } 
        
    }
}