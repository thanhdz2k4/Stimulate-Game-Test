using System;
using System.Collections.Generic;

namespace Patterns.StateMachine.TThanh
{
    public class StateMachin<T> where T : System.Enum
    {
        private IActionState<T> _currentState;
        private Dictionary<T, IActionState<T>> _states = new Dictionary<T, IActionState<T>>();

        public event Action<T, T> OnStateChanged;
        public T CurrentStateType { get; private set; }

        public void AddState(T stateType, IActionState<T> state)
        {
            if (!_states.ContainsKey(stateType))
            {
                _states[stateType] = state;
            }
        }

        public void ChangeState(T newStateType)
        {
            if (_states.ContainsKey(newStateType) && !EqualityComparer<T>.Default.Equals(CurrentStateType, newStateType))
            {
                T previousStateType = CurrentStateType;

                _currentState?.OnExit();
                _currentState = _states[newStateType];
                CurrentStateType = newStateType;
                _currentState?.OnEnter();

                OnStateChanged?.Invoke(previousStateType, newStateType);
            }
        }

        public void Update()
        {
            _currentState?.OnUpdate();
        }

        public void FixedUpdate()
        {
            _currentState?.OnFixedUpdate();
        }

        public bool CanChangeState()
        {
            return _currentState?.CanExit() ?? true;
        }

        public IActionState<T> GetCurrentState()
        {
            return _currentState;
        }

    }
    public interface IActionState<T> where T : System.Enum
    {
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
        bool CanExit();
        bool IsCompleted();
    }

}

