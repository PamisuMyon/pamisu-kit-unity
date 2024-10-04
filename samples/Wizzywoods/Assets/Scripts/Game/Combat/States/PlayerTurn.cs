
using Game.Events;
using PamisuKit.Common;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class PlayerTurn : Base
        {
            public PlayerTurn(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Owner.On<RequestEndPlayerTurn>(OnRequestEndPlayerTurn);
            }

            public override void OnExit()
            {
                base.OnExit();
                Owner.Off<RequestEndPlayerTurn>(OnRequestEndPlayerTurn);
            }

            private void OnRequestEndPlayerTurn(RequestEndPlayerTurn e)
            {
                Machine.ChangeState<EnemyTurn>();
            }
            
        }
    }
}