using AbilitySystem;
using AbilitySystem.Authoring;
using AttributeSystem.Components;
using GameplayTag.Authoring;

namespace Pamisu.GASExtension
{
    public class AbilitySystemCharacterEx : AbilitySystemCharacter
    {
        // Tags granted directly to character
        public GameplayTagContainer TagContainer; // { get; private set; }

        private void Start()
        {
            TagContainer = new GameplayTagContainer();
            if (_attributeSystem == null)
                _attributeSystem = GetComponentInChildren<AttributeSystemComponent>();
        }

        public AbstractAbilitySpec GetAbilitySpec<T>()
        {
            var type = typeof(T);
            foreach (var it in GrantedAbilities)
            {
                if (it.Ability.GetType() == type)
                {
                    return it;
                }
            }
            return null;
        }
        
        public AbstractAbilitySpec GetAbilitySpec(GameplayTagScriptableObject tag)
        {
            foreach (var it in GrantedAbilities)
            {
                if (it.Ability.AbilityTags.AssetTag == tag)
                {
                    return it;
                }
            }
            return null;
        }

        public bool TryActivateAbility<T>() where T : AbstractAbilityScriptableObject
        {
            var spec = GetAbilitySpec<T>();
            if (spec == null) return false;
            TryActivateAbility(spec);
            return true;
        }
        
        public bool TryActivateAbility(GameplayTagScriptableObject tag)
        {
            var spec = GetAbilitySpec(tag);
            if (spec == null) return false;
            TryActivateAbility(spec);
            return true;
        }

        public void TryActivateAbility(AbstractAbilitySpec spec)
        {
            StartCoroutine(spec.TryActivateAbility());
        }
        
    }
}