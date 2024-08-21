﻿using System.Collections.Generic;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class EffectComponent : MonoBehaviour, IUpdatable
    {
        public bool IsActive => Owner.IsActive;
        public Character Owner { get; private set; }
        public List<Effect> Effects = new();
        
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
                    effect.OnRemoved();
                }
            }
        }

        public bool TryApplyEffect(Effect effect)
        {
            if (!effect.CanApply(this))
                return false;
            
            if (effect.Config.IsStackable)
            {
                for (int i = 0; i < Effects.Count; i++)
                {
                    if (Effects[i].Config.Type == effect.Config.Type
                        && Effects[i].CanApply(this))
                    {
                        Effects[i].OnStack(effect);
                        return true;
                    }
                }
            }
            
            if (effect.Config.DurationPolicy == EffectDurationPolicy.Infinite
                || effect.Config.DurationPolicy == EffectDurationPolicy.Durationnal)
            {
                Effects.Add(effect);
            }
            
            effect.OnApplied(this);

            if (effect.Config.DurationPolicy == EffectDurationPolicy.Instant)
            {
                effect.OnRemoved();
            }
            
            return true;
        }
        
    }
}