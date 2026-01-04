using System;
using System.Collections.Generic;

using UnityEngine;

namespace Templates.FSM
{
    public abstract class FSM : IFSM
    {
        public IFSMState CurrentState { get; private set; }
        private readonly Dictionary<Type, IFSMState> _states = new Dictionary<Type, IFSMState>();

        public virtual void InitializeState(IFSMState state)
        {
            if (_states.ContainsKey(state.GetType()))
            {
                Debug.LogError($"Condition {state} has already been registered ");
                return;
            }

            _states.Add(state.GetType(), state);
        }

        public virtual T GetState<T>() where T : IFSMState
        {
            Type type = typeof(T);

            if (_states.TryGetValue(type, out IFSMState state))
                return (T)state;
            else
                Debug.LogError($"Condition {type} is not registered.");

            return default;
        }

        public virtual void SwitchStateTo<T>() where T : IFSMState
        {
            Type type = typeof(T);

            if (CurrentState?.GetType() == type)
            {
                Debug.LogWarning($"Already in {typeof(T)} state.");
                return;
            }

            if (_states.TryGetValue(type, out IFSMState newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }
            else
                Debug.LogError($"Condition {type} is not registered.");
        }

        public virtual void Update()
        {
            CurrentState?.Update();
        }
    }
}
