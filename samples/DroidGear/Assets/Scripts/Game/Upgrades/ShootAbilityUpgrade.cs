using Game.Abilities;
using Game.Configs;
using Game.Configs.Upgrades;
using Game.Framework;
using UnityEngine;

namespace Game.Upgrades
{
    public class ShootAbilityUpgrade : Upgrade, IAttributeModifier
    {

        private ShootAbilityUpgradeConfig _config;
        
        public ShootAbilityUpgrade(UpgradeConfig config) : base(config)
        {
            _config = config as ShootAbilityUpgradeConfig;
        }
        
        public override void OnApplied(UpgradeComponent comp)
        {
            base.OnApplied(comp);
            Owner.AttrComp.AddModifier(this);
            Owner.AttrComp.Refresh();

            var ability = Owner.GetAttackAbility() as ShootAbility;
            Debug.Assert(ability != null, "Attack ability is null", Owner.Go);
            var config = ability.Config as ShootAbilityConfig;
            var emitters = config!.Emitters;
            for (int i = 0; i < emitters.Length; i++)
            {
                emitters[i].AttributeDict[AttributeType.BranchCount] += _config.BranchCountAddend;
                emitters[i].AttributeDict[AttributeType.BurstCount] += _config.BurstCountAddend;
                if (emitters[i].Projectile != null)
                {
                    emitters[i].Projectile.ExplosionRadiusMultiplier += _config.ExplosionRadiusMultiplier;
                }
            }
        }
        
        public override void OnRemoved()
        {
            base.OnRemoved();
            var ability = Owner.GetAttackAbility() as ShootAbility;
            Debug.Assert(ability != null, "Attack ability is null", Owner.Go);
            var config = ability.Config as ShootAbilityConfig;
            var emitters = config!.Emitters;
            for (int i = 0; i < emitters.Length; i++)
            {
                emitters[i].AttributeDict[AttributeType.BranchCount] -= _config.BranchCountAddend;
                emitters[i].AttributeDict[AttributeType.BurstCount] -= _config.BurstCountAddend;
                if (emitters[i].Projectile != null)
                {
                    emitters[i].Projectile.ExplosionRadiusMultiplier -= _config.ExplosionRadiusMultiplier;
                }
            }
            
            Owner.AttrComp.RemoveModifier(this);
            Owner.AttrComp.Refresh();
        }

        public float GetMultiplier(AttributeType attributeType)
        {
            if (attributeType == AttributeType.Damage)
                return _config.DamageMultiplier;
            return 0;
        }

        public float GetAddend(AttributeType attributeType)
        {
            return 0;
        }
    }
}