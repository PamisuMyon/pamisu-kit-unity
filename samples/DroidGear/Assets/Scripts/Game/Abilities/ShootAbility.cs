using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Framework;
using Game.Props.Projectiles;
using UnityEngine;

namespace Game.Abilities
{
    public class ShootAbility : Ability
    {

        private Character _target;
        private ShootAbilityConfig _config;

        public ShootAbility(AbilityConfig config) : base(config)
        {
            _config = config as ShootAbilityConfig;
        }

        public override void SetTarget(AbilityTargetInfo info)
        {
            _target = info.MainTarget;
        }

        public override void ClearTarget()
        {
            _target = null;
        }

        protected override async UniTask DoActivate(CancellationToken cancellationToken)
        {
            if (_target == null)
            {
                Debug.LogError($"Ability {Config.Id} do activate failed: Target is null");
                return;
            }

            // act
            await this.ProcessEmitters(_config.Emitters, _config.EmitMethod, cancellationToken);

            // cooldown affected by attack speed
            var attackSpeed = Owner.AttrComp[AttributeType.AttackSpeed].Value;
            if (attackSpeed != 0)
                Cooldown = Config.Cooldown / attackSpeed;
            else
                Cooldown = Config.Cooldown;
        }

    }
}