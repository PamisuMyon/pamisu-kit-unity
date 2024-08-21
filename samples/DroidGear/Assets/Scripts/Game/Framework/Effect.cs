
using Game.Configs;
using UnityEngine.Windows.WebCam;

namespace Game.Framework
{
    public enum EffectDurationPolicy
    {
        Instant, 
        Infinite, 
        Durationnal, 
        Periodic
    }
    
    public abstract class Effect
    {
        
        public float DurationRemain { get; protected set; }
        public float DurationTotal { get; protected set; }
        public float PeriodCounter { get; protected set; }
        public int StackNum { get; protected set; }

        public EffectConfig Config { get; protected set; }
        public Character Owner { get; protected set; }
        public Character Instigator { get; protected set; }

        public Effect(EffectConfig config, Character instigator = null)
        {
            Config = config;
            Instigator = instigator;
        }
        
        public virtual void OnApplied(Character owner)
        {
            Owner = owner;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            if (Config.DurationPolicy == EffectDurationPolicy.Durationnal)
            {
                
            }
            else if (Config.DurationPolicy == EffectDurationPolicy.Periodic)
            {
                
            }
        }

        public virtual void OnStack(Effect effect)
        {
        }

        public virtual void OnRemoved()
        {
            Owner = null;
        }
        
        public virtual bool CanApply()
        {
            return true;
        }
        
    }
}