using UnityEngine;

namespace Pamisu
{
    public class PlayerInput : MonoBehaviour
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

        public float LookHorizontal
        {
            get { return GetLookAxis("Mouse X"); }
        }

        public float LookVertical => InvertMouseY? -GetLookAxis("Mouse Y") : GetLookAxis("Mouse Y");

        public bool Enabled { get; set; }
        
        private void Start()
        {
            Enabled = true;
        }

        private void Update()
        {
            if (!Enabled) return;
            
            _movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            _movement = Vector3.ClampMagnitude(_movement, 1);
            
            if (Input.GetButtonDown("Jump"))
                Jump = true;
            
            Sprint = Input.GetButton("Sprint");
            
            if (Input.GetButtonDown("Interact"))
                InteractDown = true;

            if (Input.GetButtonDown("Fire1"))
                Fire1Down = true;
            if (Input.GetButtonDown("Fire2"))
                Fire2Down = true;
            if (Input.GetButtonDown("Fire3"))
                Fire3Down = true;

        }
        
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