using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using Pamisu.Gameplay.Platformer;
using Pamisu.Platformer2D.Abilities;
using UnityEngine;

namespace Pamisu.Platformer2D.Player
{
    public class PlayerController : PlatformerPlayerController2D
    {

        public Character Character;
        
        [Header("Ability")]
        public AbstractAbilityScriptableObject[] Abilities;
        public AbstractAbilityScriptableObject[] InitialisationAbilities;
        [SerializeField]
        private GameplayTagScriptableObject customMovementTag;

        private AbstractAbilitySpec[] abilitySpecs;

        private void Start()
        {
            if (Character == null)
                Character = GetComponent<Character>();
            
            ActivateInitialisationAbilities();
            GrantAbilities();
        }
        
        protected override void Update()
        {
            HandleAbilityInputs();
            
            if (Character.TagContainer.HasTag(customMovementTag))
                return;
            
            Movement.GroundedCheck();

            if (Input.Move.x > 0)
                Character.SetOrientation(CharacterOrientation.Right);
            else if (Input.Move.x < 0)
                Character.SetOrientation(CharacterOrientation.Left);
        }

        protected override void FixedUpdate()
        {
            if (Character.TagContainer.HasTag(customMovementTag))
                return;
            
            if (Input.Jump)
            {
                Movement.Jump();
                Input.Jump = false;
            }

            if (Input.JumpHeld && Movement.CanJumpHeld)
            {
                Movement.JumpHeld();
            }

            Movement.Move(Input.Move.x);
            Movement.ApplyVelocity();
        }

        private void ActivateInitialisationAbilities()
        {
            for (var i = 0; i < InitialisationAbilities.Length; i++)
            {
                var spec = InitialisationAbilities[i].CreateSpec(Character);
                Character.GrantAbility(spec);
                StartCoroutine(spec.TryActivateAbility());
            }
        }

        private void GrantAbilities()
        {
            abilitySpecs = new AbstractAbilitySpec[Abilities.Length];
            for (var i = 0; i < Abilities.Length; i++)
            {
                var spec = Abilities[i].CreateSpec(Character);
                Character.GrantAbility(spec);
                abilitySpecs[i] = spec;
            }
        }

        private void HandleAbilityInputs()
        {
            if (Input.Interact)
            {
                Character.TryActivateAbility<DashAbility>();
                Input.Interact = false;
            }
        }
    }
}