using Game.Combat;
using Game.Common;
using Game.Configs;
using Game.Framework;

namespace Game.Effects
{
    public class ColdEffect : Effect, IAttributeModifier
    {
        public ColdEffect(EffectConfig config, Character instigator = null) : base(config, instigator)
        {
        }
        
        public override void OnApplied(EffectComponent comp)
        {
            base.OnApplied(comp);
            Owner.AttrComp.AddModifier(this);
            Owner.AttrComp.Refresh();
            
            var meshEffector = Owner.Model.MeshEffector;
            if (meshEffector != null)
                meshEffector.ChangeEffect(MeshEffector.EffectType.Ice);
        }

        protected override void OnPeriodExecute()
        {
            base.OnPeriodExecute();
            if (!AttributeDict.TryGetValue(AttributeType.Damage, out var damageValue))
                damageValue = 0;
            if (damageValue != 0)
            {
                var damage = new Damage(Instigator, -damageValue);
                DamageHelper.ApplyDamage(damage, Owner);
            }
        }

        public override void OnStack(EffectConfig config)
        {
            base.OnStack(config);
            DurationRemain = Config.Duration;
        }

        public override void OnRemoved()
        {
            var meshEffector = Owner.Model.MeshEffector;
            if (meshEffector != null)
                meshEffector.ChangeEffect(MeshEffector.EffectType.None);
            
            Owner.AttrComp.RemoveModifier(this);
            base.OnRemoved();
        }

        public float GetMultiplier(AttributeType attributeType)
        {
            if (AttributeDict.TryGetValue(attributeType, out var value))
                return value;
            return 0;
        }

        public float GetAddend(AttributeType attributeType)
        {
            return 0;
        }
        
    }
}