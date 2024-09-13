using PamisuKit.Framework;
using UnityEngine;

namespace CustomUpdate
{
    public class CustomUpdateObject : MonoEntity, IUpdatable
    {

        private float _moveSpeed;
        private Vector3 _moveDirection;
        private float _rotateSpeed;
        private Vector3 _rotateAxis;

        protected override bool AutoSetupOverride => false;

        protected override void OnCreate()
        {
            base.OnCreate();
            _moveSpeed = Random.Range(CustomUpdateConstants.MoveSpeedMin, CustomUpdateConstants.MoveSpeedMax);
            var dir = Random.insideUnitCircle;
            _moveDirection = new Vector3(dir.x, 0f, dir.y).normalized;
            _rotateSpeed = Random.Range(CustomUpdateConstants.RotateSpeedMin, CustomUpdateConstants.RotateSpeedMin);
            _rotateAxis = Random.onUnitSphere;
            
            var posX = Random.Range(CustomUpdateConstants.BoundaryXMin, CustomUpdateConstants.BoundaryXMax);
            var posZ = Random.Range(CustomUpdateConstants.BoundaryZMin, CustomUpdateConstants.BoundaryZMax);
            Trans.position = new Vector3(posX, 1f, posZ);

            Trans.rotation = Random.rotation;
        }

        public void OnUpdate(float deltaTime)
        {
            var pos = Trans.position;
            if (pos.x < CustomUpdateConstants.BoundaryXMin
                || pos.x > CustomUpdateConstants.BoundaryXMax)
                _moveDirection.x = -_moveDirection.x;
            if (pos.z < CustomUpdateConstants.BoundaryZMin
                || pos.z > CustomUpdateConstants.BoundaryZMax)
                _moveDirection.z = -_moveDirection.z;
            
            Trans.Translate(_moveSpeed * deltaTime * _moveDirection, Space.World);
            Trans.Rotate(_rotateAxis, _rotateSpeed);
        }
        
    }
}
