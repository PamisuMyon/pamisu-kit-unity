using UnityEngine;

namespace CustomUpdate
{
    public class NativeUpdateObject : MonoBehaviour
    {
        
        private float _moveSpeed;
        private Vector3 _moveDirection;
        private float _rotateSpeed;
        private Vector3 _rotateAxis;

        private void Awake()
        {
            _moveSpeed = Random.Range(CustomUpdateConstants.MoveSpeedMin, CustomUpdateConstants.MoveSpeedMax);
            var dir = Random.insideUnitCircle;
            _moveDirection = new Vector3(dir.x, 0f, dir.y).normalized;
            _rotateSpeed = Random.Range(CustomUpdateConstants.RotateSpeedMin, CustomUpdateConstants.RotateSpeedMin);
            _rotateAxis = Random.onUnitSphere;
            
            var posX = Random.Range(CustomUpdateConstants.BoundaryXMin, CustomUpdateConstants.BoundaryXMax);
            var posZ = Random.Range(CustomUpdateConstants.BoundaryZMin, CustomUpdateConstants.BoundaryZMax);
            transform.position = new Vector3(posX, 1f, posZ);

            transform.rotation = Random.rotation;
        }

        private void Update()
        {
            var pos = transform.position;
            if (pos.x < CustomUpdateConstants.BoundaryXMin
                || pos.x > CustomUpdateConstants.BoundaryXMax)
                _moveDirection.x = -_moveDirection.x;
            if (pos.z < CustomUpdateConstants.BoundaryZMin
                || pos.z > CustomUpdateConstants.BoundaryZMax)
                _moveDirection.z = -_moveDirection.z;
            
            transform.Translate(_moveSpeed * Time.deltaTime * _moveDirection, Space.World);
            transform.Rotate(_rotateAxis, _rotateSpeed);
        }
        
    }
}