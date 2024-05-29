using UnityEngine;
using UnityEngine.AI;

namespace Game.Common
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentDebugVisualizer : MonoBehaviour
    {

        [SerializeField]
        private Color _lineColor = Color.red;

        [SerializeField]
        private Color _dotColor = Color.black;

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            if (!_agent.hasPath)
                return;
            var points = _agent.path.corners;
            Gizmos.color = _dotColor;
            Gizmos.DrawSphere(points[0], .1f);
            for (int i = 1; i < points.Length; i++)
            {
                Debug.DrawLine(points[i - 1], points[i], _lineColor);
                Gizmos.DrawSphere(points[i], .1f);
            }
        }

    }
}