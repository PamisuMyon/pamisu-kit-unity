using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Effects;
using Game.Framework;
using Game.Props;
using PamisuKit.Common.Util;
using PamisuKit.Framework;

namespace Game.Abilities
{
    public class SprayAbility : Ability, IUpdatable
    {

        private SprayAbilityConfig _config;
        private Spray _spray;
        private List<Character> _targets = new();
        
        public bool IsActive => State == AbilityState.Active;
        
        public SprayAbility(AbilityConfig config) : base(config)
        {
        }

        public override void OnGranted(AbilityComponent comp)
        {
            base.OnGranted(comp);
            _spray = Owner.GetComponentInChildren<Spray>();
            _spray.SetData(_config.ActLayer);
            _spray.AreaEnter += OnSprayAreaEnter;
            _spray.AreaExit += OnSprayAreaExit;
        }

        public override void OnRevoked()
        {
            base.OnRevoked();
            if (_spray != null)
            {
                _spray.AreaEnter -= OnSprayAreaEnter;
                _spray.AreaExit -= OnSprayAreaExit;
            }
        }

        protected override async UniTask DoActivate(CancellationToken cancellationToken)
        {
            _targets.Clear();
            _spray.Activate();
            await Region.Ticker.Delay(_config.Duration, cancellationToken);
            _spray.Cancel();
        }
        
        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                var effectComp = _targets[i].EffectComp;
                if (effectComp.TryStackEffect(_config.Effect))
                    continue;
                
                var effect = EffectFactory.Create(_config.Effect);
                effectComp.TryApplyEffect(effect);
            }
        }

        protected override void OnCanceled()
        {
            base.OnCanceled();
            _spray.Cancel();
        }

        private void OnSprayAreaEnter(Character c)
        {
            _targets.AddUnique(c);
        }
        
        private void OnSprayAreaExit(Character c)
        {
            _targets.Remove(c);
        }


    }
}