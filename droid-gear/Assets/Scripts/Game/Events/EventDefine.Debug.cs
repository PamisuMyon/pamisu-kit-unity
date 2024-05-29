using UnityEngine;

namespace Game.Events
{
#if UNITY_EDITOR

    public struct RequestDrawDebugSphere
    {
        public Vector3 Center;
        public float Radius;
        public Color Color;
        public float Duration;
        public bool IsWired;
    }

#endif
}