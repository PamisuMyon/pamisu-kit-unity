using UnityEngine;

namespace Pamisu
{
    public class PlayerInputBase : MonoBehaviour
    {
        public float LookSensitivity = 1f;
        public bool InvertMouseY = true;
        
        private Vector3 _movement;
        public Vector3 Movement => _movement;
        
        public bool Sprint { get; private set; }
        public bool Jump { get; set; }
        
        public bool InteractDown { get; set; }

        public bool Fire1Down { get; set; }
        public bool Fire2Down { get; set; }
        public bool Fire3Down { get; set; }

        public float LookHorizontal => GetLookAxis("Mouse X");

        public float LookVertical => InvertMouseY? -GetLookAxis("Mouse Y") : GetLookAxis("Mouse Y");

        protected float GetLookAxis(string axisName)
        {
            float value = Input.GetAxis(axisName);
            value *= LookSensitivity;
            value *= 0.01f;
            return value;
        }

        public void Invalidate()
        {
            _movement = Vector2.zero;
            Sprint = false;
            Jump = false;
            Fire1Down = false;
            Fire2Down = false;
            Fire3Down = false;
        }
        
        

        // public void OnApplicationFocus(bool hasFocus)
        // {
        //     if (!hasFocus) return;
        //     UnityUtil.LockCursor(true);
        // }
        
    }
}