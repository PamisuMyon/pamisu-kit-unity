using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Util;
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
        
        protected override async UniTask DoActivate(CancellationToken cancellationToken)
        {
            var originPos = Owner.Trans.position;
            // Move to target
            var targetPos = _target.Trans.position;
            var dir = Owner.Trans.position - targetPos;
            targetPos += dir.normalized * (_target.Model.VisualRadius + Owner.Model.VisualRadius);
            await MoveTowards(targetPos, 10f, cancellationToken);
            
            Owner.Model.Anim.SetTrigger(AnimConst.MeleeAttack1);
            await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            Debug.Log($"{Owner.Go} Attack ");
            await UniTask.Delay(633, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);

            await MoveTowards(originPos, 10f, cancellationToken);
            _target = null;
        }

        private async UniTask MoveTowards(Vector3 targetPos, float speed, CancellationToken cancellationToken)
        {
            Vector3 pos;
            do
            {
                pos = Vector3.MoveTowards(Owner.Trans.position, targetPos, speed * Time.deltaTime);
                Owner.Trans.position = pos;
                await UniTask.Yield(cancellationToken);
            } while (!pos.Approximately(targetPos, 0.1f));
        }


    }
} 