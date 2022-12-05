using UnityEngine;

namespace Pamisu.TopDownShooter.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {

        [SerializeField]
        private PlayerController controller;
        
        private Animator anim;


        private void Start()
        {
            anim = GetComponent<Animator>();
            if (controller == null)
                controller = GetComponent<PlayerController>();
            controller.Attributes.OnDied += OnDied;
        }

        private void Update()
        {
            var localVelocity = controller.transform.InverseTransformVector(controller.Velocity);
            anim.SetFloat(AnimID.GroundSpeedX, localVelocity.x);
            anim.SetFloat(AnimID.GroundSpeedZ, localVelocity.z);
            anim.SetBool(AnimID.IsAiming, controller.IsAiming);
        }
        
        private void OnDied()
        {
            anim.SetTrigger(AnimID.Die);
        }
        
    }
}