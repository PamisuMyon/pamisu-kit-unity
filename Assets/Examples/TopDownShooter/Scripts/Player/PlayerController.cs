using Pamisu.Commons.FSM;
using Pamisu.Gameplay.TopDown;
using Pamisu.TopDownShooter.Player.States;
using UnityEngine;

namespace Pamisu.TopDownShooter.Player
{
    [RequireComponent(typeof(MonoStateMachine))]
    public class PlayerController : TwinStickShooterPlayerController
    {

        [Header("Spacesuit Player")]
        [Header("Attack")]
        [SerializeField]
        public Transform FirePoint;
        [SerializeField]
        public float FireInterval = .5f;
        [SerializeField]
        public GameObject ProjectilePrefab;
        [SerializeField]
        public float AimAnimationDelay = 0.1f;
        
        [Header("Cannon")]
        [SerializeField]
        public float CannonCooldown = 20f;
        [SerializeField]
        public GearCannon CannonPrefab;
        public float CannonCooldownCounter { get; private set; }
        
        public MonoStateMachine Machine { get; private set; }
        public Blackboard Blackboard { get; set; }
        public ActorAttributes Attributes { get; private set; }

        private void Awake()
        {
            Attributes = GetComponent<ActorAttributes>();
            Attributes.OnDied += OnDied;
        }

        protected override void Start()
        {
            base.Start();
            Blackboard = new Blackboard();
            Machine = GetComponent<MonoStateMachine>();
            Machine.AddState(new NormalState(this));
            Machine.AddState(new PreAimState(this));
            Machine.AddState(new ShootState(this));
            Machine.AddState(new DeathState(this));
            Machine.ChangeState<NormalState>();
            
            GameManager.Instance.OnPause += () =>
            {
                Input.Asset.Player.Disable();
                Input.Asset.Menu.Enable();
            };
            GameManager.Instance.OnResume += () =>
            {
                Input.Asset.Player.Enable();
                Input.Asset.Menu.Disable();
            };
        }

        private void OnDied()
        {
            Machine.ChangeState<DeathState>();
        }

        private void Update()
        {
            if (CannonCooldownCounter > 0)
            {
                CannonCooldownCounter -= Time.deltaTime;
            }
        }

        public void HandleCannon()
        {
            if (Input.Interact)
            {
                if (CannonCooldownCounter > 0) return;
                
                var go = Instantiate(CannonPrefab);
                go.Spawn(transform);
                CannonCooldownCounter = CannonCooldown;
                Input.Interact = false;
            }
        }
        
    }
}