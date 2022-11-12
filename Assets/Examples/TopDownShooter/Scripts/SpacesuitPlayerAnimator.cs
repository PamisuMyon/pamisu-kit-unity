using UnityEngine;

namespace Pamisu.Examples.TopDownShooter
{
    [RequireComponent(typeof(Animator))]
    public class SpacesuitPlayerAnimator : MonoBehaviour
    {

        [SerializeField]
        private SpacesuitPlayerController _playerController;
        
        private Animator _anim;

        private void Start()
        {
            _anim = GetComponent<Animator>();
            if (_playerController == null)
                _playerController = GetComponent<SpacesuitPlayerController>();
        }

        private void Update()
        {
            _anim.SetFloat("GroundSpeedX", _playerController.Velocity.x);
            _anim.SetFloat("GroundSpeedZ", _playerController.Velocity.x);
            _anim.SetBool("IsAiming", _playerController.IsAiming);
        }
        
    }
}