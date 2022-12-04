using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pamisu.Commons.FSM
{
    public class MonoStateMachine : MonoBehaviour, IStateMachine
    {

        [Header("Debug")] 
        public bool EnableLog = true;
        public bool EnableGizmos = true;

        private StateMachine machine = new StateMachine();

        public IStateMachine Machine => machine;

        public string MachineName
        {
            get => machine.MachineName;
            set => machine.MachineName = value;
        }

        private void OnEnable()
        {
            machine.EnableLog = EnableLog;
        }

        public Dictionary<Type, IState> States => machine.States;

        public IState CurrentState => machine.CurrentState;

        public IState PreviousState => machine.PreviousState;
        
        public void AddState(IState state)
        {
            machine.AddState(state);
        }

        public void AddState<T>() where T : IState, new()
        {
            machine.AddState<T>();
        }

        public void RemoveState<T>() where T : IState
        {
            machine.RemoveState<T>();
        }

        public bool HasState<T>() where T : IState
        {
            return machine.HasState<T>();
        }

        public void ChangeState(Type type)
        {
            machine.ChangeState(type);
        }

        public void ChangeState<T>() where T : IState
        {
            machine.ChangeState<T>();
        }

        public void OnProcess()
        {
            machine.OnProcess();
        }

        public void OnPhysicsProcess()
        {
            machine.OnPhysicsProcess();
        }

        private void Update()
        {
            machine.OnProcess();
        }

        private void FixedUpdate()
        {
            machine.OnPhysicsProcess();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!EnableGizmos || CurrentState == null) return;
            Handles.Label(transform.position, $"{MachineName} \n {CurrentState.StateName}");
        }
#endif
    }
}