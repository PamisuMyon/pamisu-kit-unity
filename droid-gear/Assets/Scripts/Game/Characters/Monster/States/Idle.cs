using Cysharp.Threading.Tasks;
using Game.Common;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Idle : Base
        {
            public Idle(MonsterController owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.Agent.enabled = true;
                Owner.Agent.isStopped = true;
                Owner.Model.Anim.SetBool(AnimConst.IsRunning, false);
                DelaySelectTarget().Forget();
            }

            private async UniTaskVoid DelaySelectTarget()
            {
                await UniTask.Yield(Owner.destroyCancellationToken);
                Owner.SelectTarget();
                if (Bb.Target != null)
                    Machine.ChangeState<Track>();
            }
            
        }
    }
}
