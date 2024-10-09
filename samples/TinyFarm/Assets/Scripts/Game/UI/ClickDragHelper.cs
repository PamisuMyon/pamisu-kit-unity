using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystem = Game.Inputs.InputSystem;

namespace Game.UI
{
    public class ClickDragHelper : MonoEntity, IUpdatable
    {
        [SerializeField]
        private Vector2 _cursorOffset;
        
        [SerializeField]
        private Camera _uiCam;
        
        [SerializeField]
        private RectTransform _rootRectTrans;

        private Vector3 _dragOffset;
        private InputSystem _inputSystem;
        
        public bool IsDragging { get; private set; }
        
        public IDragDummy DragDummy { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _inputSystem = GetSystem<InputSystem>();
            _inputSystem.Actions.Game.CursorConfirm.performed += OnCursorConfirmPerformed;
            _inputSystem.Actions.Game.CursorCancel.performed += OnCursorCancelPerformed;
            
            RegisterService(this);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            RemoveService(this);

            if (_inputSystem != null)
            {
                _inputSystem.Actions.Game.CursorConfirm.performed -= OnCursorConfirmPerformed;
                _inputSystem.Actions.Game.CursorCancel.performed -= OnCursorCancelPerformed;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsDragging)
                return;
            if (DragDummy == null)
                return;
            
            var cursorPos = _inputSystem.Actions.Game.CursorPosition.ReadValue<Vector2>();
            var cursorWorldPos = _uiCam.ScreenToWorldPoint(cursorPos);
            cursorWorldPos.z = 0f;
            DragDummy.Trans.position = cursorWorldPos + _dragOffset;
        }
        
        private void OnCursorConfirmPerformed(InputAction.CallbackContext c)
        {
            
        }
        
        private void OnCursorCancelPerformed(InputAction.CallbackContext c)
        {
            if (!IsDragging)
                return;
            EndDrag();
        }

        public void BeginDrag(IDragDummy dragDummy)
        {
            DragDummy = dragDummy;
            DragDummy.Trans.SetParent(Trans);
            IsDragging = true;
        }

        public void EndDrag()
        {
            IsDragging = false;
            DragDummy.OnEndDrag();
            DragDummy = null;
            _dragOffset = Vector3.zero;
        }
        
    }
}