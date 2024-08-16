using System;
using UnityEngine;

namespace PamisuKit.Framework
{
    public class MonoTriggerBridge : MonoBridge
    {
        public event Action<Collider> TriggerEnterEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(other);
        }
        
    }
}