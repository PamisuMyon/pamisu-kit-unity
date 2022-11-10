using UnityEngine;

namespace Pamisu.Player
{
    
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputBase))]
    public class TopDownPlayerControllerBase : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private float turnSpeed = 360f;
        
        protected CharacterController cc;
        protected PlayerInputBase input;

        protected void Start()
        {
            cc = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputBase>();
        }
        
        
    }
}