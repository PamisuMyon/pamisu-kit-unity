using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Framework;

namespace Game.Combat.Abilities.Spec
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

        protected override UniTask DoActivate(CancellationToken cancellationToken)
        {
            // var enemy = GameObject.Find("Enemy_Mushnub(Clone)");
            // _target = enemy.GetComponent<Enemy>();
            // // TODO Block UI events
            // Owner.View.Anim.SetTrigger(AnimConst.CastShoot);
            // // pre-delay
            // await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            // // shoot
            // var go = await CombatSystem.Instance.Pooler.Spawn("Assets/Res/Projectiles/Proj_Mage_Attack.prefab");
            // var proj = go.GetComponent<HomingProjectile>();
            // proj.SetupEntity(Owner.Region);
            // await proj.Perform(Owner, _target, Owner.View.FirePoints[0].position);
            // proj.OnRelease();
            // CombatSystem.Instance.Pooler.Release(proj.Go);
            // // post-delay
            // await UniTask.Delay(633, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            // _target = null;
            return UniTask.CompletedTask;
        }
    }
}