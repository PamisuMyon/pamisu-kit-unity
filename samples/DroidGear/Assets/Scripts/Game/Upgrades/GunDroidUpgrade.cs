using Game.Abilities;
using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Modifiers
{
    public class GunDroidUpgrade : Upgrade, IAttributeModifier
    {
        private GunDroidUpgradeConfig _config;
        
        public GunDroidUpgrade(UpgradeConfig config) : base(config)
        {
            _config = config as GunDroidUpgradeConfig;
        }

        public override void OnApplied(UpgradeComponent comp)
        {
            base.OnApplied(comp);
            Owner.AttrComp.AddModifier(this);
            Owner.AttrComp.Refresh();

            var ability = Owner.GetAttackAbility() as SimpleShootAbility;
            Debug.Assert(ability != null, "Attack ability is null", Owner.Go);
            var config = ability.Config as ShootAbilityConfig;
            var emitters = config!.Emitters;
            for (int i = 0; i < emitters.Length; i++)
            {
                emitters[i].BranchCount += _config.BranchCountAddend;
                emitters[i].BurstCount += _config.BurstCountAddend;
            }
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            var ability = Owner.GetAttackAbility() as SimpleShootAbility;
            Debug.Assert(ability != null, "Attack ability is null", Owner.Go);
            var config = ability.Config as ShootAbilityConfig;
            var emitters = config!.Emitters;
            for (int i = 0; i < emitters.Length; i++)
            {
                emitters[i].BranchCount -= _config.BranchCountAddend;
                emitters[i].BurstCount -= _config.BurstCountAddend;
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