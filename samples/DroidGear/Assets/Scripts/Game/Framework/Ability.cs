using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;

namespace Game.Framework
{
    public enum AbilityState
    {
        None,
        Disabled,
        Inactive,
        Active,
    }
    
    public abstract class Ability
    {
        
        protected CancellationTokenSource CtsActivate; 
        
        public AbilityConfig Config { get; protected set; }
        public AbilityComponent Comp { get; protected set; }
        public Character Owner => Comp.Owner;
        public AbilityState State { get; protected set; } = AbilityState.None;
        public float Cooldown { get; internal set; }
        public bool IsCooldownManaged { get; protected set; } = true;

        public Ability(AbilityConfig config)
        {
            Config = config;
        }
            
        public virtual void OnGranted(AbilityComponent comp)
        {
            Comp = comp;
            State = AbilityState.Inactive;
        }

        public virtual void OnRevoked()
        {
            Comp = null;
            State = AbilityState.None;
        }

        public virtual bool CanActivate()
        {
            return State == AbilityState.Inactive
                   && CheckTags() 
                   && CheckCost() 
                   && CheckCooldown();
        }

        public virtual async UniTask<bool> TryActivate(CancellationToken cancellationToken = default)
        {
            if (!CanActivate())
                return false;
            await Activate(cancellationToken);
            return true;
        }

        public virtual async UniTask Activate(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
            {
                CtsActivate?.Cancel();
                CtsActivate = new CancellationTokenSource();
                cancellationToken = CtsActivate.Token;
            }
            State = AbilityState.Active;
            await DoActivate(cancellationToken);
            State = AbilityState.Inactive;
        }

        protected abstract UniTask DoActivate(CancellationToken cancellationToken);
        
        public virtual void Cancel()
        {
            if (State == AbilityState.Active)
            {
                CtsActivate?.Cancel();
                State = AbilityState.Inactive;
            }
        }

        protected virtual bool CheckTags()
        {
            return true;
        }

        protected virtual bool CheckCost()
        {
            return true;
        }

        protected virtual bool CheckCooldown()
        {
            return Cooldown <= 0;
        }

        public virtual void SetTarget(AbilityTargetInfo info) {}

        public virtual void ClearTarget() {}

    }
    
}