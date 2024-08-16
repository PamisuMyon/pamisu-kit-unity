using System.Collections.Generic;
using Game.Events;
using PamisuKit.Common;
using UnityEngine;

namespace Game.Common
{

    public class EnvQuerier
    {

        private readonly Collider[] _tempResults;
        private readonly List<Vector3> _tempPoints = new();

        public EnvQuerier(int colliderContainerSize = 8)
        {
            _tempResults = new Collider[colliderContainerSize];
        }

        public bool CircleSpheresQuery(
            out Vector3 result,
            Vector3 center, 
            int sampleCount, 
            float radius, 
            float sphereRadius, 
            Vector3 referPoint,
            int obstacleLayerMask,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore,
            bool randomStartAngle = true
            // bool isNearest = true
            )
        {
            result = center;

            _tempPoints.Clear();
            radius += sphereRadius;
            var angleDelta = 2f * Mathf.PI / sampleCount;
            var angle = randomStartAngle? Random.Range(0, 2f * Mathf.PI) : 0f;
            for (int i = 0; i < sampleCount; i++)
            {
                var sphereCenter = center + new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
                var num = Physics.OverlapSphereNonAlloc(sphereCenter, sphereRadius, _tempResults, obstacleLayerMask, queryTriggerInteraction);
#if UNITY_EDITOR
                EventBus.Emit(new RequestDrawDebugSphere 
                {
                    Center = sphereCenter,
                    Radius = sphereRadius,
                    Color = new Color(.1f, .5f, 1f, .7f),
                    Duration = .5f
                });
#endif
                if (num == 0) 
                {
                    _tempPoints.Add(sphereCenter);
                }
                angle += angleDelta;
            }

            if (_tempPoints.Count == 0)
                return false;

            var minDisSqr = float.PositiveInfinity;
            for (int i = 0; i < _tempPoints.Count; i++)
            {
                var dir = _tempPoints[i] - referPoint;
                var disSqr = dir.sqrMagnitude;
                if (disSqr < minDisSqr)
                {
                    minDisSqr = disSqr;
                    result = _tempPoints[i];
                }
            }

            return true;
        }

    }
}