using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pamisu.Commons.FSM
{
    public class MonoStateMachine : MonoBehaviour, IStateMachine
    {

        [Header("Debug")] 
        public bool LogEnabled = true;
        public bool GizmosEnabled = true;
        
        private IStateMachine _machine = new StateMachine();

        public IStateMachine StateMachine => _machine;

        public string MachineName
        {
            get => _machine.MachineName;
            set => _machine.MachineName = value;
        }

        public Dictionary<Type, IState> States => _machine.States;

        public IState CurrentState => _machine.CurrentState;

        public IState PreviousState => _machine.PreviousState;
        
        public void AddState(IState state)
        {
            _machine.AddState(state);
        }

        public void AddState<T>() where T : IState, new()
        {
            _machine.AddState<T>();
        }

        public void RemoveState<T>() where T : IState
        {
            _machine.RemoveState<T>();
        }

        public bool HasState<T>() where T : IState
        {
            return _machine.HasState<T>();
        }

        public void ChangeState(Type type)
        {
            _machine.ChangeState(type);
        }

        public void ChangeState<T>() where T : IState
        {
            _machine.ChangeState<T>();
        }

        public void SetCurrentState<T>() where T : IState
        {
            _machine.SetCurrentState<T>();
        }

        public void OnProcess()
        {
            _machine.OnProcess();
        }

        public void OnPhysicsProcess()
        {
            _machine.OnPhysicsProcess();
        }

        private void Update()
        {
            _machine.OnProcess();
        }

        private void FixedUpdate()
        {
            _machine.OnPhysicsProcess();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (CurrentState == null) return;
            Handles.Label(transform.position, $"{MachineName} \n {CurrentState.StateName}");
        }
#endif
    }
}