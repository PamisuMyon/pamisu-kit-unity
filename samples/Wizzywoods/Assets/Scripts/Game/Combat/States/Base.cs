using PamisuKit.Common.FSM;
using UnityEngine;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public abstract class Base : State
        {

            protected CombatSystem Owner;
            protected Blackboard Bb => Owner.Bb;

            public Base(CombatSystem owner)
            {
                Owner = owner;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Debug.Log($"CombatStates {GetType().Name} OnEnter");
            }
            
            public override void OnExit()
            {
                base.OnExit();
                Debug.Log($"CombatStates {GetType().Name} OnExit");
            }
            
        }
    }
}