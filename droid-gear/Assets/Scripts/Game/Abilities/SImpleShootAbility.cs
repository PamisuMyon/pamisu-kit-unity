using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Abilities
{
    public class SimpleShootAbility : Ability
    {

        private Character _target;
        
        public SimpleShootAbility(AbilityConfig config) : base(config)
        {
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

            Owner.Model.Anim.SetTrigger(AnimConst.RangeAttack1);

            // pre-delay
            await UniTask.Delay(TimeSpan.FromSeconds(Config.ActPreDelay), 
                DelayType.DeltaTime, 
                PlayerLoopTiming.Update, 
                cancellationToken);

            // act
            Cooldown = Config.Cooldown;
            // var proj = await CombatSystem.Instance.Pooler.Spawn<HomingProjectile>(Config.PrefabRes);
            // proj.SetupEntity(Owner.Region);
            // await proj.Perform(Owner, _target, Owner.Model.FirePoints[0].position, Owner.AttrComp[AttributeType.Damage].Value);
            // CombatSystem.Instance.Pooler.Release(proj);

            // post-delay
            await UniTask.Delay(TimeSpan.FromSeconds(Config.ActPostDelay), 
                DelayType.DeltaTime, 
                PlayerLoopTiming.Update, 
                cancellationToken);

            // cooldown affected by attack speed
            Cooldown = Config.Cooldown / Owner.AttrComp[AttributeType.AttackSpeed].Value;
        }
        
    }
}