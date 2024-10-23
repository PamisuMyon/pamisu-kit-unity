using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Playground
{
    public class NavTest : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _agent;

        private Camera _mainCam;
        
        private void Start()
        {
            _mainCam = Camera.main;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.enabled = true;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var cursorPos = Mouse.current.position.ReadValue();
                var ray = _mainCam.ScreenPointToRay(cursorPos);
                var hit = Physics2D.GetRayIntersection(ray, 100f);
                Debug.DrawRay(ray.origin, ray.direction * 100f, hit ? Color.green : Color.red);
                if (hit)
                {
                    var pos = hit.point;
                    _agent.destination = pos;
                    _agent.isStopped = false;
                }
            }
        }
        
    }
}