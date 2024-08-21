using Game.Common;
using Game.Configs;
using Game.Framework;

namespace Game.Effects
{
    public class ColdEffect : Effect
    {
        public ColdEffect(EffectConfig config, Character instigator = null) : base(config, instigator)
        {
        }
        
        public override void OnApplied(EffectComponent comp)
        {
            base.OnApplied(comp);
            
            var meshEffector = Comp.Owner.Model.MeshEffector;
            if (meshEffector != null)
                meshEffector.ChangeEffect(MeshEffector.EffectType.Ice);
        }

        public override void OnRemoved()
        {
            var meshEffector = Comp.Owner.Model.MeshEffector;
            if (meshEffector != null)
                meshEffector.ChangeEffect(MeshEffector.EffectType.None);
            base.OnRemoved();
        }
    }
}