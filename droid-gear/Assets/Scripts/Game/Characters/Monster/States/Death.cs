using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Common;
using UnityEngine;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Death : Base
        {
            public Death(MonsterController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.Agent.isStopped = true;
                
                Owner.Chara.Model.Anim.SetTrigger(AnimConst.Death);
                Sink().Forget();
                // DropItems().Forget();
            }

            private async @bool Sink()
            {
                // hard-code
                await UniTask.Delay(1500, false, PlayerLoopTiming.Update, Owner.destroyCancellationToken);

                var targetPos = Owner.Trans.position;
                targetPos.y -= Owner.Model.VisualHeight - .5f;
                // TODO

                await UniTask.Delay(1000, false, PlayerLoopTiming.Update, Owner.destroyCancellationToken);
                // TODO Return to pool
                Object.Destroy(Owner.gameObject);
            }

            // private async UniTaskVoid DropItems()
            // {
            //     // TODO TEMP
            //     var pickup = await CombatSystem.Instance.Pooler.Spawn<Pickup>("Assets/Res/Props/CrystalShards/CrystalShard_1.prefab");
            //     pickup.SetupEntity(Owner.Region);
            //     pickup.SetData(CombatSystem.Instance.Pooler);
            //     var pos = Owner.Trans.position;
            //     var rand = RandomUtil.InsideAnnulus(Owner.Model.VisualRadius, Owner.Model.VisualRadius + .2f);
            //     pos.x += rand.x;
            //     pos.z += rand.y;
            //     pickup.Trans.position = pos;
            // }
            
        }
    }
}
