using DG.Tweening;
using Pamisu.Commons;
using Pamisu.Commons.FSM;
using Pamisu.Gameplay;
using Pamisu.TopDownShooter.Enemies.States;
using UnityEngine;
using UnityEngine.AI;

namespace Pamisu.TopDownShooter.Enemies
{
    [RequireComponent(typeof(ActorAttributes), typeof(MonoStateMachine), typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {

        [SerializeField]
        public float WalkSpeed = 5f;
        [SerializeField]
        public float RunSpeed = 8f;
        [SerializeField]
        public float IdleTime = 5f;
        [SerializeField]
        public float IdleTimeRandomness = .5f;
        [SerializeField]
        public float PatrolDistance = 10f;
        [SerializeField]
        public float TrackInterval = .3f;
        [SerializeField]
        public float HitAndRunDistance = 5f;
        [SerializeField]
        public TriggerArea DetectArea;

        [Space]
        [SerializeField]
        private EnemyAction[] actions; 
        [SerializeField]
        public Transform[] FirePoints;

        [Space]
        public float ModelHalfHeight = .5f;

        public Animator Animator { get; private set; }
        public ActorAttributes Attributes { get; private set; }
        public MonoStateMachine Machine { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public EnemySpawner Spawner { get; set; }
        
        public Blackboard Blackboard { get; private set; }

        private void Awake()
        {
            Attributes = GetComponent<ActorAttributes>();
            Attributes.OnDied += OnDied;
        }

        private void Start()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            DetectArea.TriggerEnter += OnDetectAreaEnter;
            
            foreach (var it in actions)
            {
                it.Owner = this;
            }
            
            Blackboard = new Blackboard();
            Machine = GetComponent<MonoStateMachine>();
            InitMachine();
        }

        private void Update()
        {
            foreach (var action in actions)
            {
                if (action.CooldownCounter > 0)
                    action.CooldownCounter -= Time.deltaTime;
            }
            if (Blackboard.ActionBlockCounter > 0)
                Blackboard.ActionBlockCounter -= Time.deltaTime;

            Animator.SetFloat(AnimID.GroundSpeed, Agent.velocity.sqrMagnitude);
        }

        protected virtual void InitMachine()
        {
            Machine.AddState(new NoneState(this));
            Machine.AddState(new IdleState(this));
            Machine.AddState(new MoveState(this));
            Machine.AddState(new TrackState(this));
            Machine.AddState(new ActionState(this));
            Machine.AddState(new DeathState(this));
            Machine.ChangeState<IdleState>();
        }

        public void Spawn(float spawnHeight, Vector3 targetPosition)
        {
            Machine.ChangeState<NoneState>();
            var pos = targetPosition;
            pos.y = spawnHeight;
            transform.position = pos; 
            transform.DOMove(targetPosition, 1f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Machine.ChangeState<IdleState>();
                });
        } 

        private void OnDied()
        {
            if (Spawner != null)
                Spawner.OnEnemyKilled(this);
            Machine.ChangeState<DeathState>();
        }
        
        private void OnDetectAreaEnter(Collider obj)
        {
            if (obj.CompareTag("Player"))
            {
                DetectArea.TriggerEnter -= OnDetectAreaEnter;
                DetectArea.gameObject.SetActive(false);
                Blackboard.TrackTarget = obj.GetComponent<ActorAttributes>();
                Machine.ChangeState<TrackState>();
            }
        }
        
        public bool RotateTowards(Vector3 direction, float rotateSpeed = 360)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            return transform.rotation.Approximately(targetRotation, 0.001f);
        }

        public bool GetAvailableAction(float targetDistanceSqr, out EnemyAction action)
        {
            action = null;
            var isBlocking = Blackboard.IsActionBlocking;
            for (var i = actions.Length - 1; i >= 0; i--)
            {
                if (actions[i].IsInCoolDown) continue;
                if (!actions[i].CheckTargetDistanceSqr(targetDistanceSqr)) continue;
                if (!isBlocking || !actions[i].CanBeBlocked)
                {
                    action = actions[i];
                    return true;
                }
            }
            return false;
        }

        public float GetTargetStoppingDistance()
        {
            if (actions.Length == 0) return 5f;
            return actions[0].MaxTargetDistance;
        }
        
    }
}