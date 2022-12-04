using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Pamisu.Commons
{
    public static class UnityUtil
    {
        public static Vector3 NIV3 = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// 判断两个向量是否近似相等
        /// </summary>
        public static bool Approximately(this Vector2 a, Vector2 b, float epsilon = .05f)
        {
            return a.x.Approximately(b.x, epsilon) && a.y.Approximately(b.y, epsilon);
        }

        public static bool Approximately(this Vector3 a, Vector3 b, float epsilon = .05f)
        {
            return a.x.Approximately(b.x, epsilon) && a.y.Approximately(b.y, epsilon) && a.z.Approximately(b.z, epsilon);
        }

        public static bool Approximately(this float a, float b, float epsilon = .05f)
        {
            return Mathf.Abs(a - b) < epsilon;
        }

        public static bool Approximately(this Quaternion a, Quaternion b, float epsilon = .05f)
        {
            return Mathf.Abs(Quaternion.Dot(a, b)) > 1 - epsilon;
        }

        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public static void SetGlobalScale (this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3 (globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        public static void GizmosDrawCircle(Vector3 center, float radius, Color color = default, int vertices = 50)
        {
            float deltaTheta = (2f * Mathf.PI) / vertices;
            float theta = 0f;
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Vector3 oldPos = center + pos;
            if (color != default)
                Gizmos.color = color;
            for (int i = 0; i < vertices + 1; i++)
            {
                pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                Gizmos.DrawLine(oldPos, center + pos);
                oldPos = center + pos;
                theta += deltaTheta;
            }
        }

        public static GameObject GetRootObject(string name)
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObj = scene.GetRootGameObjects();
            foreach (GameObject obj in rootObj)
            {
                if (obj.name == name)
                {
                    return obj;
                }
            }
            return null;
        }

        public static bool IsParentOf(this Transform parent, Transform target)
        {
            while (target.parent != null)
            {
                if (target.parent == parent)
                    return true;
                target = target.parent;
            }
            return false;
        }

        public static int ToLayer(this LayerMask layerMask)
        {
            return Mathf.RoundToInt(Mathf.Log(layerMask.value, 2f));
        }

        public static IEnumerator Delay(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }
        
        public static IEnumerator DelayFrame(Action action, int frames = 1)
        {
            while (frames > 0)
            {
                frames--;
                yield return null;
            }
            action?.Invoke();
        }

        public static Vector3 Clamp(this Bounds bounds, Vector3 point)
        {
            point.x = Mathf.Clamp(point.x, bounds.min.x, bounds.max.x);
            point.y = Mathf.Clamp(point.y, bounds.min.y, bounds.max.y);
            point.z = Mathf.Clamp(point.z, bounds.min.z, bounds.max.z);
            return point;
        }

        public static void LockCursor(bool isLock)
        {
            if (isLock)
            {
# if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.Confined;
#else
                Cursor.lockState = CursorLockMode.Locked;
#endif
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        public static bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result, int sampleCount = 10)
        {
            for (var i = 0; i < sampleCount; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;
                if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
        
    }
}