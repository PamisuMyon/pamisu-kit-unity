using Cysharp.Threading.Tasks;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class EnemyTurn : Base
        {
            public EnemyTurn(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Process().Forget();
            }

            private async UniTaskVoid Process()
            {
                // for (var i = 0; i < Bb.EnemiesOnStage.Count; i++)
                // {
                //     await Bb.EnemiesOnStage[i].Act();
                // }
                Machine.ChangeState<PlayerTurn>();
            }
            
        }
    }
}