using UnityEngine;

namespace Game.Framework
{
    public class CameraController : MonoBehaviour
    {

        [Header("Shake")]
        [SerializeField, Range(0, 1f)]
        private float _decay = .8f;
        [SerializeField]
        private Vector3 _maxPosOffset = new(5f, 5f, 5f);
        [SerializeField]
        [Tooltip("pitch, yaw, roll in degrees")]
        private Vector3 _maxAngleOffset = new(60f, 60f, 60f);
        [SerializeField]
        private float _traumaPower = 2f;

        private bool _hasTarget;
        private float _selfOriginY;
        private Vector3 _camOriginPos;
        private Vector3 _camOriginAngle;
        private float _trauma;

        private Transform _target;
        public Transform Target 
        { 
            get => _target;
            set 
            {
                _target = value;
                _hasTarget = _target != null;
            }
        }
        public Camera Cam { get; private set; }

        private void Awake()
        {
            Cam = GetComponentInChildren<Camera>();
            _camOriginPos = Cam.transform.localPosition;
            _camOriginAngle = Cam.transform.localRotation.eulerAngles;
            _selfOriginY = transform.position.y;
        }

        private void Update()
        {
            if (_trauma > 0)
            {
                _trauma = Mathf.Max(0, _trauma - _decay * Time.deltaTime);
                Shake();
            }
        }

        private void LateUpdate()
        {
            if (_hasTarget)
            {
                var pos = _target.position;
                pos.y = _selfOriginY;
                transform.position = pos;
            }
        }

        public void AddTrauma(float delta)
        {
	        _trauma = Mathf.Clamp01(_trauma + delta);
        }

        private void Shake()
        {
            var amount = Mathf.Pow(_trauma, _traumaPower);
            var angleOffset = Vector3.zero;
            var posOffset = Vector3.zero;
            angleOffset.x = _maxAngleOffset.x * amount * Random.Range(-1f, 1f);
            angleOffset.y = _maxAngleOffset.y * amount * Random.Range(-1f, 1f);
            angleOffset.z = _maxAngleOffset.z * amount * Random.Range(-1f, 1f);
            posOffset.x = _maxPosOffset.x * amount * Random.Range(-1f, 1f);
            posOffset.y = _maxPosOffset.y * amount * Random.Range(-1f, 1f);
            posOffset.z = _maxPosOffset.z * amount * Random.Range(-1f, 1f);
            Cam.transform.SetLocalPositionAndRotation(_camOriginPos + posOffset, Quaternion.Euler(_camOriginAngle + angleOffset));
        }

    }
}
