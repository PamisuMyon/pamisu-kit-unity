using Game.Common;
using Game.Configs;
using Game.Framework;

namespace Game.Effects
{
    public class ColdEffect : Effect, IAttributeModifier
    {
        private float _moveSpeedMultiplier;
        
        public ColdEffect(EffectConfig config, Character instigator = null) : base(config, instigator)
        {
        }

        public void SetAttributes(float moveSpeedMultiplier)
        {
            _moveSpeedMultiplier = moveSpeedMultiplier;
        }
        
        public override void OnApplied(EffectComponent comp)
        {
            base.OnApplied(comp);
            Owner.AttrComp.AddModifier(this);
            
            var meshEffector = Owner.Model.MeshEffector;
            if (meshEffector != null)
                meshEffector.ChangeEffect(MeshEffector.EffectType.Ice);
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
            if (attributeType == AttributeType.MoveSpeed)
                return _moveSpeedMultiplier;
            return 0;
        }

        public float GetAddend(AttributeType attributeType)
        {
            return 0;
        }
        
    }
}