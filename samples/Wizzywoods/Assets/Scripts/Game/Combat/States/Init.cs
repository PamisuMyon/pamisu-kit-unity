using Cysharp.Threading.Tasks;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        
        public class Init : Base
        {
            public Init(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                DoInit().Forget();
            }

            private async UniTaskVoid DoInit()
            {
                await Owner.InitPlayer();
                await Owner.LoadEnemies();
                Machine.ChangeState<PlayerTurn>();
            }
        }
        
    }
}