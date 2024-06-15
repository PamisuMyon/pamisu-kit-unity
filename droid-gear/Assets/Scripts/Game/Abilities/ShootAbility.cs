using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Configs;
using Game.Framework;
using Game.Props;
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

            if (Owner.Model.Anim != null)
                Owner.Model.Anim.SetTrigger(AnimConst.RangedAttack1);

            // pre-delay
            if (Config.ActPreDelay != 0)
                await Owner.Region.Ticker.Delay(Config.ActPreDelay, cancellationToken);

            // act
            var damage = new Damage(Owner, -Owner.AttrComp[AttributeType.Damage].Value);
            var firePoint = Owner.Model.FirePoints[0];
            var direction = _target.Trans.position - firePoint.position;
            direction.y = 0;

            var pooler = Owner.GetDirector<GameDirector>().Pooler;
            var proj = await pooler.Spawn<Projectile>(Config.PrefabRes.RuntimeKey.ToString());
            proj.Setup(Owner.Region);
            proj.Activate(damage, firePoint.position, direction, Config.ActLayer);

            // post-delay
            if (Config.ActPostDelay != 0)
                await Owner.Region.Ticker.Delay(Config.ActPostDelay, cancellationToken);

            // cooldown affected by attack speed
            var attackSpeed = Owner.AttrComp[AttributeType.AttackSpeed].Value;
            if (attackSpeed != 0)
                Cooldown = Config.Cooldown / attackSpeed;
            else
                Cooldown = Config.Cooldown;
        }

    }
}