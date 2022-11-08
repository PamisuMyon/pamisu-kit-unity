using System;
using System.Collections.Generic;

namespace Pamisu.Common.FSM
{
    public interface IStateMachine
    {
        
        string MachineName { get; set; }
        
        Dictionary<Type, IState> States { get; }
        
        IState CurrentState { get; }
        
        IState PreviousState { get; }

        void AddState(IState state);
        
        void AddState<T>() where T : IState, new();

        void RemoveState<T>() where T : IState;

        bool HasState<T>() where T : IState;

        void ChangeState(Type type);
        
        void ChangeState<T>() where T : IState;

        void SetCurrentState<T>() where T : IState;

        void OnProcess();

        void OnPhysicsProcess();

    }
}