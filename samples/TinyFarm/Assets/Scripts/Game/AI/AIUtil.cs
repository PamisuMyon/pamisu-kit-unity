using NPBehave;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.AI
{
    public static class AIUtil
    {

        public static void SetRandomPositionOnNavMesh(
            this Blackboard blackboard, 
            string key,
            Vector3 center, 
            float radius)
        {
            RandomUtil.RandomPositionOnNavMesh(center, radius, out var result);
            blackboard.Set(key, result);
        }
        
    }
}