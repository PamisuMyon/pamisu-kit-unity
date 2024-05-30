using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Configs;
using Game.Framework;
using UnityEngine;

namespace Game.Combat.Abilities.Spec
{
    public class SimpleMeleeAbility : Ability
    {
        private Character _target;
        
        public SimpleMeleeAbility(AbilityConfig config) : base(config)
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
            Owner.Model.Anim.SetTrigger(AnimConst.MeleeAttack1);
            await GameplayUtil.SimpleDelay(Config.ActPreDelay, cancellationToken);
            Debug.Log($"{Owner.Go} Attack {_target.Go}");
            var damage = new Damage(Owner, -Owner.AttrComp[AttributeType.Damage].Value);
            _target.AttrComp.ChangeHealth(damage);
            await GameplayUtil.SimpleDelay(Config.ActPostDelay, cancellationToken);
            Cooldown = Config.Cooldown;
        }
        
    }
} 