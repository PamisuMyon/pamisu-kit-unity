using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Common;
using UnityEngine;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public class Death : Base
        {
            public Death(DroidController owner) : base(owner)
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

            private async UniTaskVoid Sink()
            {
                // hard-code
                await Owner.Region.Ticker.Delay(1.5f, Owner.destroyCancellationToken);

                var targetPos = Owner.Trans.position;
                targetPos.y -= Owner.Model.VisualHeight - .5f;
                // TODO

                await Owner.Region.Ticker.Delay(1f, Owner.destroyCancellationToken);
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
