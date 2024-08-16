using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PamisuKit.Common.FSM
{
    public class StateMachine : IStateMachine
    {
        
        public string MachineName { get; set; }

        public Dictionary<Type, IState> States { get ; } = new();

        public IState CurrentState { get; protected set; }
        
        public IState PreviousState { get; protected set; }
        
        public bool EnableLog { get; set; }

        public StateMachine(bool enableLog = false)
        {
            // MachineName = GetType().FullName;
            MachineName = GetType().Name;
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
            
            var newState = States[type];
            if (CurrentState == newState)
                return;

            PreviousState = CurrentState;
            CurrentState = newState;
            if (PreviousState != null)
            {
                if (EnableLog)
                    Debug.Log($"{MachineName} {PreviousState.StateName} OnExit");
                PreviousState.OnExit();
            }
            if (EnableLog)
                Debug.Log($"{MachineName} {CurrentState.StateName} OnEnter");
            CurrentState.OnEnter();
        }
        
        public void ChangeState<T>() where T : IState
        {
            ChangeState(typeof(T));
        }

        public void OnUpdate(float deltaTime)
        {
            CurrentState.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            CurrentState.OnFixedUpdate(deltaTime);
        }

        public void OnLateUpdate(float deltaTime)
        {
            CurrentState.OnLateUpdate(deltaTime);
        }

        public void OnDestroy()
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                CurrentState = null;
            }
        }
        
#if UNITY_EDITOR
        public void OnDrawGizmos(Vector3 position)
        {
            if (CurrentState == null) return;
            Handles.Label(position, $"{MachineName} \n {CurrentState.StateName}");
        }
#endif
        
    }
}