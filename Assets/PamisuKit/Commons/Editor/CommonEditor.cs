using UnityEditor;
using UnityEngine;

namespace Pamisu.Commons.Editor
{
    public class CommonEditor : MonoBehaviour
    {

        [MenuItem("GameObject/Snap to Ground", isValidateFunction: true)]
        public static bool ValidateSnapToGround()
        {
            return Selection.activeTransform != null;
        }
        
        /**
         * Snap selected object down to the first collider detected, the object must have a collider
         */
        [MenuItem("GameObject/Snap to Ground")]
        public static void SnapToGround()
        {
            var go = Selection.activeTransform.gameObject;
            var hits = Physics.RaycastAll(go.transform.position, Vector3.down, float.PositiveInfinity);
            for (var i = 0; i < hits.Length; i++)
            {
                if (!hits[i].transform.IsChildOf(go.transform)
                    && go.transform != hits[i].transform)
                {
                    var backHits = Physics.RaycastAll(hits[i].point, Vector3.up, float.PositiveInfinity);
                    for (var j = 0; j < backHits.Length; j++)
                    {
                        if (go.transform == backHits[j].transform 
                            || backHits[j].transform.IsChildOf(go.transform))
                        {
                            var height = go.transform.position.y - backHits[j].point.y;
                            var snapPoint = hits[i].point;
                            snapPoint.y += height;
                            go.transform.position = snapPoint;
                            break;
                        }
                    }
                    break;
                }
            }
        }
        
    }
}