using UnityEngine;

namespace Pamisu.TopDownShooter.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {

        [SerializeField]
        private PlayerController Controller;
        
        private Animator anim;


        private void Start()
        {
            anim = GetComponent<Animator>();
            if (Controller == null)
                Controller = GetComponent<PlayerController>();
            Controller.Attributes.OnDied += OnDied;
        }

        private void Update()
        {
            var localVelocity = Controller.transform.InverseTransformVector(Controller.Velocity);
            anim.SetFloat(AnimID.GroundSpeedX, localVelocity.x);
            anim.SetFloat(AnimID.GroundSpeedZ, localVelocity.z);
            anim.SetBool(AnimID.IsAiming, Controller.IsAiming);
        }
        
        private void OnDied()
        {
            anim.SetTrigger(AnimID.Die);
        }
        
    }
}