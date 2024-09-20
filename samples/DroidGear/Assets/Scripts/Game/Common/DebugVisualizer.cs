using System.Collections.Generic;
using PamisuKit.Common;
using Game.Events;
using UnityEngine;

namespace Game.Common
{
    public class DebugVisualizer : MonoBehaviour
    {
#if UNITY_EDITOR

        private List<ReqDrawDebugSphere> _spheres = new();

        private void Start()
        {
            EventBus.On<ReqDrawDebugSphere>(OnReqDrawDebugSphere);
        }

        private void OnDestroy()
        {
            EventBus.Off<ReqDrawDebugSphere>(OnReqDrawDebugSphere);
        }

        private void OnReqDrawDebugSphere(ReqDrawDebugSphere e)
        {
            _spheres.Add(e);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            if (_spheres.Count < 0)
                return;
            for (int i = _spheres.Count - 1; i >= 0 ; i--)
            {
                var e = _spheres[i];
                DrawSphere(e);
                e.Duration -= Time.deltaTime;
                if (e.Duration < 0)
                    _spheres.RemoveAt(i);
                else
                    _spheres[i] = e;
            }
        }

        private void DrawSphere(ReqDrawDebugSphere e)
        {
            Gizmos.color = e.Color;
            if (e.IsWired)
                Gizmos.DrawWireSphere(e.Center, e.Radius);
            else
                Gizmos.DrawSphere(e.Center, e.Radius);
        }

        private void DrawCapsule()
        {

        }
#endif
    }
}