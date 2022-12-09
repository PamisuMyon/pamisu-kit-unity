using AbilitySystem;
using AbilitySystem.Authoring;
using AttributeSystem.Components;
using GameplayTag.Authoring;

namespace Pamisu.GASExtension
{
    public class AbilitySystemCharacterEx : AbilitySystemCharacter
    {
        // Tags granted directly to character
        public GameplayTagContainer TagContainer { get; private set; }

        private void Start()
        {
            TagContainer = new GameplayTagContainer();
            if (_attributeSystem == null)
                _attributeSystem = GetComponentInChildren<AttributeSystemComponent>();
        }

        public bool TryActivateAbility<T>() where T : AbstractAbilityScriptableObject
        {
            var type = typeof(T);
            foreach (var it in GrantedAbilities)
            {
                if (it.Ability.GetType() == type)
                {
                    ActivateAbility(it);
                    return true;
                }
            }
            return false;
        }
        
        public bool TryActivateAbility(GameplayTagScriptableObject tag)
        {
            foreach (var it in GrantedAbilities)
            {
                if (it.Ability.AbilityTags.AssetTag == tag)
                {
                    ActivateAbility(it);
                    return true;
                }
            }
            return false;
        }

        protected void ActivateAbility(AbstractAbilitySpec spec)
        {
            StartCoroutine(spec.TryActivateAbility());
        }
        
    }
}