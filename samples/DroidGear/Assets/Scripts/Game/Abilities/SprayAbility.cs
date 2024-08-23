using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
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

        protected Dictionary<AttributeType, float> AttributeDict = new();
        
        public bool IsActive => State == AbilityState.Active;
        
        public SprayAbility(AbilityConfig config) : base(config)
        {
            _config = config as SprayAbilityConfig;
            foreach (var it in _config.AttributeDict)
            {
                AttributeDict[it.Key] = it.Value;
            }
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
            if (Owner.Model.Anim != null)
                Owner.Model.Anim.SetBool(AnimConst.IsShooting, true);
            _targets.Clear();
            _spray.Activate();
            await Region.Ticker.Delay(_config.Duration, cancellationToken);
            _spray.Cancel();
            _targets.Clear();
            if (Owner.Model.Anim != null)
                Owner.Model.Anim.SetBool(AnimConst.IsShooting, false);
            
            Cooldown = Config.Cooldown;
        }
        
        public void OnUpdate(float deltaTime)
        {
            AttributeDict[AttributeType.Damage] = Owner.AttrComp[AttributeType.Damage].Value * _config.DamageScale;
            for (int i = 0; i < _targets.Count; i++)
            {
                var effectComp = _targets[i].EffectComp;
                if (effectComp.TryStackEffect(_config.Effect, out var appliedEffect))
                {
                    appliedEffect.SetAttributes(AttributeDict);
                    continue;
                }
                
                var effect = EffectFactory.Create(_config.Effect);
                effect.SetAttributes(AttributeDict);
                effectComp.TryApplyEffect(effect);
            }
        }

        protected override void OnCanceled()
        {
            base.OnCanceled();
            _spray.Cancel();
            _targets.Clear();
            if (Owner.Model.Anim != null)
                Owner.Model.Anim.SetBool(AnimConst.IsShooting, false);
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