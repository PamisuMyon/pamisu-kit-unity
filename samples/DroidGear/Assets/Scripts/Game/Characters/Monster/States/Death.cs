using Cysharp.Threading.Tasks;
using Game.Common;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Death : Base
        {

            private int _originalLayer;

            public Death(MonsterController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                _originalLayer = Owner.Chara.BodyCollider.gameObject.layer;
                Owner.Chara.BodyCollider.gameObject.layer = LayerMask.NameToLayer("Void");

                Owner.Agent.isStopped = true;
                Owner.Agent.enabled = false;
                
                Owner.Chara.Model.Anim.SetTrigger(AnimConst.Death);
                Sink().Forget();
                // DropItems().Forget();
            }

            public override void OnExit()
            {
                base.OnExit();
                Owner.Chara.BodyCollider.gameObject.layer = _originalLayer;
            }

            private async UniTaskVoid Sink()
            {
                // hard-code
                await Owner.Region.Ticker.Delay(1.5f, Owner.destroyCancellationToken);
                var originalPos = Owner.Trans.position;
                var targetPos = originalPos;
                targetPos.y -= Owner.Model.VisualHeight - .5f;
                
                await LMotion.Create(originalPos, targetPos, 1f)
                    .BindToPosition(Owner.Trans)
                    .ToUniTask(Owner.destroyCancellationToken);
                // if NavMeshAgent is enabled, it will back to the NavMesh when tween finished

                Owner.Chara.Died?.Invoke(Owner.Chara);
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
