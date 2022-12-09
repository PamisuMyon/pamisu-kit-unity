using System.Collections;
using AbilitySystem;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using Pamisu.Gameplay.Platformer;
using Pamisu.GASExtension;
using UnityEngine;

namespace Pamisu.Platformer2D.Abilities
{
    [CreateAssetMenu(menuName = "Platformer 2D/Abilities/Dash Ability")]
    public class DashAbility : AbstractAbilityScriptableObject
    {

        public GameplayTagScriptableObject CustomMovementTag;
        public float DashSpeed = 20f;
        public float DashDuration = .2f;
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemCharacter owner)
        {
            var spec = new DashAbilitySpec(this, owner);
            return spec;
        }
    }

    public class DashAbilitySpec : AbstractAbilitySpecEx
    {

        private DashAbility ability;
        private Character character;
        private PlatformerMovement2D movement;
        
        public DashAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemCharacter owner) : base(ability, owner)
        {
            this.ability = ability as DashAbility;
            character = owner as Character;
            movement = owner.GetComponent<PlatformerMovement2D>();
        }

        public override void CancelAbility()
        {
        }

        public override bool CheckGameplayTags()
        {
            return true;
        }

        protected override IEnumerator PreActivate()
        {
            yield return null;
        }

        protected override IEnumerator ActivateAbility()
        {
            character.TagContainer.AppendTag(ability.CustomMovementTag);
            
            var duration = ability.DashDuration;
            while (duration > 0)
            {
                movement.TargetVelocity = Vector2.right * ability.DashSpeed;
                movement.ApplyVelocity();
                
                duration -= Time.deltaTime;
                yield return null;
            }
            
            character.TagContainer.RemoveTag(ability.CustomMovementTag);
        }
        
    }
}