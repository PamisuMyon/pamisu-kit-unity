using System.Collections.Generic;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class EffectComponent : MonoBehaviour, IUpdatable
    {
        public bool IsActive => Owner.IsActive;
        public Character Owner { get; private set; }
        public Dictionary<EffectType, Effect> EffectDict { get; private set; } = new();
        public List<Effect> Effects { get; private set; } = new();
        
        
        public void Init(Character owner)
        {
            Owner = owner;
        }
        
        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].OnUpdate(deltaTime);
            }
            CleanEffects();
        }

        public void CleanEffects()
        {
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                if (Effects[i].Config.DurationPolicy == EffectDurationPolicy.Durationnal
                    && Effects[i].DurationRemain <= 0)
                {
                    var effect = Effects[i];
                    Effects.RemoveAt(i);
                    EffectDict.Remove(effect.Config.Type);
                    effect.OnRemoved();
                }
            }
        }

        public bool TryGetEffect(EffectType type, out Effect effect)
        {
            return EffectDict.TryGetValue(type, out effect);
        }
        
        public bool TryApplyEffect(Effect effect)
        {
            if (!effect.CanApply(this))
                return false;

            if (TryStackEffect(effect.Config))
                return true;
            
            if (effect.Config.DurationPolicy == EffectDurationPolicy.Infinite
                || effect.Config.DurationPolicy == EffectDurationPolicy.Durationnal)
            {
                Effects.Add(effect);
                EffectDict[effect.Config.Type] = effect;
            }
            
            effect.OnApplied(this);

            if (effect.Config.DurationPolicy == EffectDurationPolicy.Instant)
            {
                effect.OnRemoved();
            }
            
            return true;
        }

        public bool TryStackEffect(EffectConfig config)
        {
            if (EffectDict.TryGetValue(config.Type, out var appliedEffect))
            {
                appliedEffect.OnStack(config);
                return true;
            }
            return false;
        }
        
    }
}