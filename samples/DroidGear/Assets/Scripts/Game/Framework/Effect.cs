
using Game.Configs;
using UnityEngine;

namespace Game.Framework
{
    public enum EffectDurationPolicy
    {
        Instant,
        Infinite,
        Durationnal,
    }
    
    public abstract class Effect
    {
        
        public float DurationRemain { get; protected set; }
        public float PeriodCounter { get; protected set; }
        public int StackNum { get; protected set; }

        public EffectConfig Config { get; protected set; }
        public EffectComponent Comp { get; protected set; }
        public Character Owner => Comp.Owner;
        public Character Instigator { get; protected set; }

        public Effect(EffectConfig config, Character instigator = null)
        {
            Config = config;
            Instigator = instigator;
        }
        
        public virtual void OnApplied(EffectComponent comp)
        {
            Comp = comp;
            if (Config.DurationPolicy == EffectDurationPolicy.Durationnal)
                DurationRemain = Mathf.Max(0, Config.Duration);
            else
                DurationRemain = -1;
            PeriodCounter = Config.PeriodExecuteWhenApply? 0 : Config.Period;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            if (Config.DurationPolicy == EffectDurationPolicy.Durationnal)
            {
                DurationRemain -= deltaTime;
            }
            
            if (Config.Period > 0)
            {
                if (PeriodCounter <= 0)
                {
                    OnPeriodExecute();
                    PeriodCounter = Config.Period;
                }
                PeriodCounter -= deltaTime;
            }
        }

        protected virtual void OnPeriodExecute()
        {
        }

        public virtual void OnStack(EffectConfig config)
        {
        }

        public virtual void OnRemoved()
        {
            Comp = null;
        }
        
        public virtual bool CanApply(EffectComponent comp)
        {
            return true;
        }
        
    }
}