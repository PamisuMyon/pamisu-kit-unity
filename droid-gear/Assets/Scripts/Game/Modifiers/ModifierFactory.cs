using Game.Combat.Abilities.Spec;
using Game.Configs;
using Game.Framework;

namespace Game.Modifiers
{
    public static class ModifierFactory
    {

        public static IModifier Create(ModifierConfig config)
        {
            // return config.Type switch
            // {
            //     AbilityType.SimpleShoot => new SimpleShootAbility(config),
            //     AbilityType.SimpleMelee => new SimpleMeleeAbility(config),
            //     AbilityType.Shoot => new ShootAbility(config),
            //     _ => null
            // };
            return null;
        }

    }
}