using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pamisu.Commons.FSM
{
    public class StateMachine : IStateMachine
    {
        
        public string MachineName { get; set; }

        private Dictionary<Type, IState> states = new();

        public Dictionary<Type, IState> States => states;
        
        public IState CurrentState { get; protected set; }
        
        public IState PreviousState { get; protected set; }
        
        public bool EnableLog { get; set; }

        public StateMachine(bool enableLog = false)
        {
            MachineName = GetType().ToString();
            EnableLog = enableLog;
        }
        
        public void AddState(IState state)
        {
            state.Machine = this;
            state.OnAddToMachine();
            States.Add(state.GetType(), state);
        }

        public void AddState<T>() where T : IState, new()
        {
            if (HasState<T>())
            {
                Debug.LogWarning($"State {typeof(T)} already exists.");
                return;
            }

            var state = new T();
            state.Machine = this;
            state.OnAddToMachine();
            States.Add(typeof(T), state);
        }

        public void RemoveState<T>() where T : IState
        {
            var type = typeof(T);
            if (States.ContainsKey(type))
                States.Remove(type);
        }

        public bool HasState<T>() where T : IState
        {
            return States.ContainsKey(typeof(T));
        }

        public void ChangeState(Type type)
        {
            if (!States.ContainsKey(type))
            {
                Debug.LogError($"Cannot change to state {type}, it does not exist in the state machine.");
                return;
            }

            PreviousState = CurrentState;
            if (CurrentState != null)
            {
                if (EnableLog)
                    Debug.Log($"{MachineName} {CurrentState.StateName} OnLeave");
                CurrentState.OnExit();
            }
            CurrentState = States[type];
            if (EnableLog)
                Debug.Log($"{MachineName} {CurrentState.StateName} OnEnter");
            CurrentState.OnEnter();
        }
        
        public void ChangeState<T>() where T : IState
        {
            ChangeState(typeof(T));
        }

        public void OnProcess()
        {
            CurrentState.OnProcess();
        }

        public void OnPhysicsProcess()
        {
            CurrentState.OnPhysicsProcess();
        }
        
    }
}