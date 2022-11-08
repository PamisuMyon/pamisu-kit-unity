using UnityEngine;

namespace Pamisu.Common
{
    public class TriggerArea : MonoBehaviour
    {

        public event System.Action<Collider> TriggerEnter;
        public event System.Action<Collider> TriggerStay;
        public event System.Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("OnTriggerEnter: " + other);
            TriggerEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            // Debug.Log("OnTriggerStay: " + other);
            TriggerStay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            // Debug.Log("OnTriggerExit: " + other);
            TriggerExit?.Invoke(other);
        }
    }

}
