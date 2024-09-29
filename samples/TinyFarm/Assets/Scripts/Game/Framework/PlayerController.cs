using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class PlayerController : MonoEntity, IUpdatable
    {

        [SerializeField]
        private LayerMask _unitLayerMask;

        [SerializeField]
        private Camera _mainCam;

        [SerializeField]
        private Indicator _hoverIndicator;

        [SerializeField]
        private Indicator _selectionIndicator;

        protected override void OnCreate()
        {
            base.OnCreate();
            if (_mainCam == null)
                _mainCam = Camera.main;

            _hoverIndicator.Setup(Region);
            _selectionIndicator.Setup(Region);
        }

        public void OnUpdate(float deltaTime)
        {
            var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.GetRayIntersection(ray, 100f, _unitLayerMask);
            Debug.DrawRay(ray.origin, ray.direction * 100f, hit? Color.green : Color.red);
            if (hit && hit.collider.TryGetComponent(out Unit unit)
                  && _selectionIndicator.AttachedUnit != unit)
            {
                _hoverIndicator.Attach(unit);
            }
            else
            {
                _hoverIndicator.Detach();
            }
        }
    }
}