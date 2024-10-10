using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class Indicator : MonoEntity
    {
        [SerializeField]
        private Transform _tl;
        
        [SerializeField]
        private Transform _tr;
        
        [SerializeField]
        private Transform _bl;
        
        [SerializeField]
        private Transform _br;
        
        public bool IsAttached { get; private set; }
        public Unit AttachedUnit { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Go.SetActive(false);
        }

        private void FitToUnit(Unit unit)
        {
            Go.SetActive(true);
            var center = unit.Trans.position + (Vector3)unit.VisualCenter;
            Trans.position = center;
            var halfSize = unit.VisualSize / 2f;
            _tl.localPosition = new Vector3(-halfSize.x, halfSize.y, 0f);
            _tr.localPosition = new Vector3(halfSize.x, halfSize.y, 0f);
            _bl.localPosition = new Vector3(-halfSize.x, -halfSize.y, 0f);
            _br.localPosition = new Vector3(halfSize.x, -halfSize.y, 0f);
        }

        public void Attach(Unit unit)
        {
            if (AttachedUnit == unit)
                return;
            FitToUnit(unit);
            IsAttached = true;
            AttachedUnit = unit;
        }
        
        public void Detach()
        {
            if (!IsAttached)
                return;
            IsAttached = false;
            AttachedUnit = null;
            Go.SetActive(false);
        }
        
    }
}