using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using PamisuKit.Common.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PamisuKit.Common.Util
{
    public static class UnityUtil
    {
        public static readonly Vector3 Niv3 = new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        public static readonly Color TransparentWhite = new(1, 1, 1, 0);
        public static readonly Color TransparentBlack = new(0, 0, 0, 0);

        /// <summary>
        /// Compares two Vectors and returns true if they are similar.
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <param name="epsilon">Tolerance</param>
        /// <returns>If two Vectors are similar.</returns>
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

        /// <summary>
        /// Clamps an angle value (in degree) between a minimum and maximum angle.
        /// </summary>
        /// <param name="angle">The angle value to clamp.</param>
        /// <param name="min">The minimum angle value.</param>
        /// <param name="max">The maximum angle value.</param>
        /// <returns>The clamped angle value between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        public static void SetGlobalScale (this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            var lossyScale = transform.lossyScale;
            transform.localScale = new Vector3 (globalScale.x / lossyScale.x, globalScale.y / lossyScale.y, globalScale.z / lossyScale.z);
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

        public static IEnumerator Repeat(float interval, Action action)
        {
            while (true)
            {
                action?.Invoke();
                yield return new WaitForSeconds(interval);
            }
        }

        public static Vector3 Clamp(this Bounds bounds, Vector3 point)
        {
            point.x = Mathf.Clamp(point.x, bounds.min.x, bounds.max.x);
            point.y = Mathf.Clamp(point.y, bounds.min.y, bounds.max.y);
            point.z = Mathf.Clamp(point.z, bounds.min.z, bounds.max.z);
            return point;
        }
        
        public static float MoveTowards(this float current, float target, float maxDistanceDelta)
        {
            if (maxDistanceDelta == 0) return current;
            var value = current + maxDistanceDelta;
            if (maxDistanceDelta < 0)
                return Mathf.Max(value, target);
            return Mathf.Min(value, target);
        }
        
        public static bool FindNearestWalkablePosition(ref Vector3 position, float maxDistance = 1f, int sampleCount = 10)
        {
            for (var i = 0; i < sampleCount; i++)
            {
                if (NavMesh.SamplePosition(position, out var hit, maxDistance, NavMesh.AllAreas))
                {
                    position = hit.position;
                    return true;
                }
            }
            return false;
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

        public static void ToggleVisibility(this CanvasGroup canvasGroup, bool visible, float visibleAlpha = 1f, bool interactable = true)
        {
            canvasGroup.alpha = visible ? visibleAlpha : 0f;
            canvasGroup.interactable = visible && interactable;
            canvasGroup.blocksRaycasts = visible && interactable;
        }
        
        public static void ToggleVisibility(this Image image, bool visible, float visibleAlpha = 1f, bool interactable = true)
        {
            image.color = visible ? new Color(1f, 1f, 1f, visibleAlpha) : TransparentWhite;
            image.raycastTarget = visible && interactable;
        }

        public static void ScreenToCanvasPoint(Canvas canvas, ref Vector2 position)
        {
            var scale = canvas.transform.localScale;
            position.x /= scale.x;
            position.y /= scale.y;
        }
        
        public static async UniTask LoadSprite(this Image image, string spriteRes, bool isTempTransparent = true, AssetRefCountMode mode = AssetRefCountMode.Single)
        {
            var originalColor = image.color;
            if (isTempTransparent)
                image.color = TransparentWhite;
            image.sprite = await AssetManager.LoadAsset<Sprite>(spriteRes, mode);
            if (isTempTransparent)
                image.color = originalColor;
        }
        
        public static async UniTask LoadSprite(this Image image, AssetReference refer, bool isTempTransparent = true, AssetRefCountMode mode = AssetRefCountMode.Single)
        {
            var originalColor = image.color;
            if (isTempTransparent)
                image.color = TransparentWhite;
            image.sprite = await AssetManager.LoadAsset<Sprite>(refer, mode);
            if (isTempTransparent)
                image.color = originalColor;
        }
        
        /// <summary>
        /// Sets a click listener for a UGUI Button with throttle time.
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="action">Click listener</param>
        /// <param name="throttle">The minimum time between consecutive clicks in seconds. </param>
        public static void SetOnClickListener(this Button button, Action action, float throttle = .2f)
        {
            button.onClick.RemoveAllListeners();
            float clickTime = 0;
            button.onClick.AddListener(() =>
            {
                if (clickTime > Time.time)
                    return;
                action.Invoke();
                clickTime = Time.time + throttle;
            });
        }
        
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                comp = go.AddComponent<T>();
            return comp;
        }

        public static bool TryGetComponentInDirectParent<T>(this GameObject go, out T comp, bool isIncludeSelf = true) 
        {
            comp = default;
            var parent = go.transform.parent;
            if (parent != null)
                parent.TryGetComponent(out comp);
            if (comp == null && isIncludeSelf)
                go.TryGetComponent(out comp);
            return comp != null;
        }

        public static bool TryGetComponentInDirectParent<T>(this Component self, out T comp, bool isIncludeSelf = true)
        {
            return TryGetComponentInDirectParent(self.gameObject, out comp, isIncludeSelf);
        } 

        public static Quaternion SmoothRotateTowards(
            this Quaternion rotation, 
            Vector3 direction, 
            float deltaTime, 
            float rotateSpeed, 
            out bool isApproximatelyEqual, 
            float epsilon = .005f)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.RotateTowards(rotation, targetRotation, rotateSpeed * deltaTime);
            isApproximatelyEqual = rotation.Approximately(targetRotation, epsilon);
            return rotation;
        }
        
        public static bool SmoothRotateTowards(
            this Transform transform, 
            Vector3 direction, 
            float deltaTime, 
            float rotateSpeed, 
            float epsilon = .005f)
        {
            var rotation = transform.rotation;
            var targetRotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.RotateTowards(rotation, targetRotation, rotateSpeed * deltaTime);
            transform.rotation = rotation;
            return rotation.Approximately(targetRotation, epsilon);;
        }
        
    }
}