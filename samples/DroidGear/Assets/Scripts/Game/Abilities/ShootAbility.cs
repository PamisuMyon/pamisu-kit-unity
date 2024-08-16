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

            // if (Owner.Model.Anim != null)
            //     Owner.Model.Anim.SetTrigger(AnimConst.RangedAttack1);

            // // pre-delay
            // if (Config.ActPreDelay != 0)
            //     await Owner.Region.Ticker.Delay(Config.ActPreDelay, cancellationToken);

            // act
            for (int i = 0; i < _config.Emitters.Length; i++)
            {
                await ProcessEmitter(_config.Emitters[i], cancellationToken);
            }

            // // post-delay
            // if (Config.ActPostDelay != 0)
            //     await Owner.Region.Ticker.Delay(Config.ActPostDelay, cancellationToken);

            // cooldown affected by attack speed
            var attackSpeed = Owner.AttrComp[AttributeType.AttackSpeed].Value;
            if (attackSpeed != 0)
                Cooldown = Config.Cooldown / attackSpeed;
            else
                Cooldown = Config.Cooldown;
        }

        private async UniTask ProcessEmitter(EmitterConfig config, CancellationToken cancellationToken)
        {
            Debug.Assert(config.FirePointIndex < Owner.Model.FirePoints.Length, "FirePointIndex out of bounds");
            var firePoint = Owner.Model.FirePoints[config.FirePointIndex];
            var startAngle = -(config.BranchCount - 1) / 2f * config.BranchAngleDelta;
            var startDirection = firePoint.forward;
            startDirection.y = 0;
            startDirection = Quaternion.AngleAxis(startAngle, Vector3.up) * startDirection;
            var rotDelta = Quaternion.AngleAxis(config.BranchAngleDelta, Vector3.up);

            var pooler = Owner.GetDirector<GameDirector>().Pooler;
            var damage = new Damage(Owner, -Owner.AttrComp[AttributeType.Damage].Value);

            // Every burst 
            for (int i = 0; i < config.BurstCount; i++)
            {
                if (config.IsPlayAnim && Owner.Model.Anim != null)
                    if (!string.IsNullOrEmpty(config.AnimTriggerParam))
                        Owner.Model.Anim.SetTrigger(config.AnimTriggerParam);
                    else
                        Owner.Model.Anim.SetTrigger(AnimConst.RangedAttack1);

                // pre-delay
                if (config.BurstPreDelay != 0)
                    await Owner.Region.Ticker.Delay(Config.ActPreDelay, cancellationToken);

                // emit projectiles
                var direction = startDirection;
                for (int j = 0; j < config.BranchCount; j++)
                {
                    var proj = await pooler.Spawn<Projectile>(config.Projectile.PrefabRef, -1, cancellationToken);
                    // if (cancellationToken.IsCancellationRequested)
                    // {
                    //     pooler.Release(proj);
                    //     throw new OperationCanceledException();
                    // }
                    proj.Setup(Owner.Region);
                    proj.SetData(
                        config.Projectile, 
                        damage, 
                        firePoint.position, 
                        direction, 
                        Config.ActLayer,
                        firePoint).Activate();

                    direction = rotDelta * direction;
                }

                // post-delay
                if (config.BurstPreDelay != 0)
                    await Owner.Region.Ticker.Delay(config.BurstPreDelay, cancellationToken);

                // interval
                if (config.BurstInterval != 0 && i != config.BurstCount - 1)
                    await Owner.Region.Ticker.Delay(config.BurstInterval, cancellationToken);
            }
        }

    }
}