using Pamisu.Common;
using UnityEngine;

namespace Pamisu
{
    public class PlayerInput : SingletonBehaviour<PlayerInput>
    {
        public float lookSensitivity = 1f;
        public bool invertMouseY = true;
        
        private Vector3 movement;
        public Vector3 Movement => movement;
        
        public bool Sprint { get; private set; }
        public bool Jump { get; set; }
        
        public bool InteractDown { get; set; }

        public bool Fire1Down { get; set; }
        public bool Fire2Down { get; set; }
        public bool Fire3Down { get; set; }

        public bool Slot1Down { get; set; }
        public bool Slot2Down { get; set; }
        public bool Slot3Down { get; set; }
        public bool Slot4Down { get; set; }

        public float LookHorizontal
        {
            get { return GetLookAxis("Mouse X"); }
        }

        public float LookVertical
        {
            get { return invertMouseY? -GetLookAxis("Mouse Y") : GetLookAxis("Mouse Y"); }
        }

        public bool Enabled { get; set; }
        
        private void Start()
        {
            Enabled = true;
        }

        private void Update()
        {
            if (!Enabled) return;
            
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            movement = Vector3.ClampMagnitude(movement, 1);
            
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

            if (Input.GetButtonDown("Slot1"))
                Slot1Down = true;
            if (Input.GetButtonDown("Slot2"))
                Slot2Down = true;
            if (Input.GetButtonDown("Slot3"))
                Slot3Down = true;
            if (Input.GetButtonDown("Slot4"))
                Slot4Down = true;
        }
        
        protected float GetLookAxis(string axisName)
        {
            float value = Input.GetAxis(axisName);
            value *= lookSensitivity;
            value *= 0.01f;
            return value;
        }

        public void Invalidate()
        {
            movement = Vector2.zero;
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