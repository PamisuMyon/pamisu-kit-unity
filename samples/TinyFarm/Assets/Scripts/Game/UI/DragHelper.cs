using Game.Inputs;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI
{
    public class DragHelper : MonoEntity, IUpdatable
    {
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
            
            RegisterService(this);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            RemoveService(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsDragging)
                return;
            if (DragDummy == null)
                return;
            
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(rootPanel, input.TouchPosition, canvas.worldCamera, out var localPoint);
            // rootPanel.position = rootPanel.transform.TransformPoint(localPoint);
            
            var cursorPos = _inputSystem.Actions.Game.CursorPosition.ReadValue<Vector2>();
            var cursorWorldPos = _uiCam.ScreenToWorldPoint(cursorPos);
            cursorWorldPos.z = 0f;
            DragDummy.Trans.position = cursorWorldPos + _dragOffset;
        }

        public void BeginDrag(IDragDummy dragDummy, Vector3 dragOffset)
        {
            DragDummy = dragDummy;
            DragDummy.Trans.parent = Trans;
            _dragOffset = dragOffset;
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