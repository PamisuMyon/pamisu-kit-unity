using System;
using System.Collections.Generic;

namespace Pamisu.Common.FSM
{
    public interface IStateMachine
    {
        float TickInterval { get; set; }
        
        Dictionary<Type, IState> States { get; set; }
        
        IState CurrentState { get; set; }

        void AddState(IState state);
        
        void AddState<T>() where T : IState, new();

        void RemoveState<T>() where T : IState;

        void ChangeState<T>() where T : IState;

        void SetCurrentState<T>() where T : IState;

        void OnProcess();

        void OnPhysicsProcess();

        
        
    }
}