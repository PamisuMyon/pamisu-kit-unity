using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Configs;
using Game.Framework;
using Game.Props.Projectiles;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Abilities
{
    public static class EmitHelper
    {

        public static async UniTask ProcessEmitter(
            Region region, 
            Transform firePoint, 
            EmitterConfig config, 
            Damage damage,
            int layer,
            CancellationToken cancellationToken, 
            Animator animator = null)
        {
            var startAngle = -(config.BranchCount - 1) / 2f * config.BranchAngleDelta;
            var startDirection = firePoint.forward;
            startDirection.y = 0;
            startDirection = Quaternion.AngleAxis(startAngle, Vector3.up) * startDirection;
            var rotDelta = Quaternion.AngleAxis(config.BranchAngleDelta, Vector3.up);

            var pooler = region.GetDirector<GameDirector>().Pooler;
            // Every burst 
            for (int i = 0; i < config.BurstCount; i++)
            {
                if (config.IsPlayAnim && animator != null)
                    if (!string.IsNullOrEmpty(config.AnimTriggerParam))
                        animator.SetTrigger(config.AnimTriggerParam);
                    else
                        animator.SetTrigger(AnimConst.RangedAttack1);

                // pre-delay
                if (config.BurstPreDelay != 0)
                    await region.Ticker.Delay(config.BurstPreDelay, cancellationToken);

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
                    proj.Setup(region);
                    proj.SetData(
                        config.Projectile, 
                        damage, 
                        firePoint.position, 
                        direction, 
                        layer,
                        firePoint).Activate();

                    direction = rotDelta * direction;
                }

                // post-delay
                if (config.BurstPreDelay != 0)
                    await region.Ticker.Delay(config.BurstPreDelay, cancellationToken);

                // interval
                if (config.BurstInterval != 0 && i != config.BurstCount - 1)
                    await region.Ticker.Delay(config.BurstInterval, cancellationToken);
            }
        }
        
        public static UniTask ProcessEmitter(
            this Ability ability, 
            EmitterConfig config, 
            CancellationToken cancellationToken)
        {
            var owner = ability.Owner;
            Debug.Assert(config.FirePointIndex < owner.Model.FirePoints.Length, "FirePointIndex out of bounds");
            var firePoint = owner.Model.FirePoints[config.FirePointIndex];
            var damage = new Damage(owner, -owner.AttrComp[AttributeType.Damage].Value);
            return ProcessEmitter(
                owner.Region, 
                firePoint, 
                config, 
                damage, 
                ability.Config.ActLayer, 
                cancellationToken, 
                owner.Model.Anim);
        }

        public static UniTask ProcessEmitter(
            this Projectile projectile,
            EmitterConfig config,
            Damage damage,
            CancellationToken cancellationToken)
        {
            return ProcessEmitter(
                projectile.Region,
                projectile.Trans,
                config,
                damage,
                projectile.gameObject.layer,
                cancellationToken);
        }


        public static async UniTask ProcessEmitters(
            this Ability ability,
            EmitterConfig[] configs,
            EmitMethod emitMethod,
            CancellationToken cancellationToken)
        {
            if (emitMethod == EmitMethod.None)
                return;
            
            if (emitMethod == EmitMethod.Sequence)
            {
                for (int i = 0; i < configs.Length; i++)
                {
                    await ProcessEmitter(ability, configs[i], cancellationToken);
                }
                return;
            }

            if (emitMethod == EmitMethod.Parallel)
            {
                var tasks = new List<UniTask>();
                for (int i = 0; i < configs.Length; i++)
                {
                    tasks.Add(ProcessEmitter(ability, configs[i], cancellationToken));
                }
                await UniTask.WhenAll(tasks);
            }
        }
        
        public static async UniTask ProcessEmitters(
            this Projectile projectile,
            EmitterConfig[] configs,
            EmitMethod emitMethod,
            Damage damage,
            CancellationToken cancellationToken)
        {
            if (emitMethod == EmitMethod.None)
                return;

            if (emitMethod == EmitMethod.Sequence)
            {
                for (int i = 0; i < configs.Length; i++)
                {
                    await ProcessEmitter(projectile, configs[i], damage, cancellationToken);
                }
                return;
            }

            if (emitMethod == EmitMethod.Parallel)
            {
                var tasks = new List<UniTask>();
                for (int i = 0; i < configs.Length; i++)
                {
                    tasks.Add(ProcessEmitter(projectile, configs[i], damage, cancellationToken));
                }
                await UniTask.WhenAll(tasks);
            }
        }
        
    }
}