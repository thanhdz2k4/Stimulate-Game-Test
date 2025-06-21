
using System;
using UnityEngine;

public class StateMachine
{
    private StateBase currentState;
    private StateBase _previousState;
    public StateBase CurrentState => currentState;
    public StateBase PreviousState => _previousState;
    public bool IsFinishedState => currentState == null;
    public Action<StateBase, StateBase> OnStateChanged;

    public void FinishState()
    {
        if (currentState != null)
            currentState.EndState();

        _previousState = currentState;
        currentState = null;
    }

    public void ChangeState(StateBase newState)
    {
        if (currentState == newState) return;
        if (currentState != null)
            currentState.EndState();

        _previousState = currentState;
        currentState = newState;
        currentState.StartState();

        OnStateChanged?.Invoke(_previousState, currentState);
    }

    public void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdateState();
    }

    public void LateUpdate()
    {
        if (currentState != null)
            currentState.LateUpdateState();
    }


}
