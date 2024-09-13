using System;
using System.Diagnostics;
using UnityEngine;

namespace CustomUpdate
{
    public class CustomUpdateDebugger : MonoBehaviour
    {
        private Stopwatch _stopwatch;
        private long _total;
        private long _num;
        private long _last;

        public bool IsEnabled { get; set; }

        public float LastTime => (float)_last / Stopwatch.Frequency * 1000f;
        public float AverageTime => (float)_total / _num / Stopwatch.Frequency * 1000f;

        private void Awake()
        {
            _stopwatch = new Stopwatch();
        }

        private void Update()
        {
            if (!IsEnabled)
                return;
            _stopwatch.Start();
        }

        private void LateUpdate()
        {
            if (!IsEnabled)
                return;
            _num++;
            _last = _stopwatch.ElapsedTicks;
            _total += _last;
            _stopwatch.Reset();
        }
        
    }
}