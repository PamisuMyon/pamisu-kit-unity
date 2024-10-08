using Game.Combat.Abilities.Spec;
using Game.Configs;
using Game.Framework;

namespace Game.Abilities
{
    public static class AbilityFactory
    {

        public static Ability Create(AbilityConfig config)
        {
            return config.Type switch
            {
                AbilityType.SimpleShoot => new SimpleShootAbility(config),
                AbilityType.SimpleMelee => new SimpleMeleeAbility(config),
                AbilityType.Shoot => new ShootAbility(config),
                AbilityType.Spray => new SprayAbility(config),
                _ => null
            };
        }
        
    }
}