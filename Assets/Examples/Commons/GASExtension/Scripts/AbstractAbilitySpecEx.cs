using System.Collections;
using AbilitySystem;
using AbilitySystem.Authoring;

namespace Pamisu.GASExtension
{
    public abstract class AbstractAbilitySpecEx : AbstractAbilitySpec
    {
        protected AbilitySystemCharacterEx OwnerEx => Owner as AbilitySystemCharacterEx;
        
        protected AbstractAbilitySpecEx(AbstractAbilityScriptableObject ability, AbilitySystemCharacter owner) : base(ability, owner)
        {
        }

        /// <summary>
        /// Try activating the ability.  Remember to use StartCoroutine() since this 
        /// is a couroutine, to allow abilities to span more than one frame.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator TryActivateAbility()
        {
            if (!CanActivateAbility()) yield break;

            isActive = true;
            OwnerEx.TagContainer.AppendTags(Ability.AbilityTags.ActivationOwnedTags);
            yield return ActivateAbility();
            EndAbility();
        }

        protected override IEnumerator PreActivate()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method to run once the ability ends
        /// </summary>
        public override void EndAbility()
        {
            base.EndAbility();
            OwnerEx.TagContainer.RemoveTags(Ability.AbilityTags.ActivationOwnedTags);
        }
        
    }
}