using UnityEngine;

public interface IStateMachine
{
    void ChangeState(State newState);
}

public class StateMachine<TState> : IStateMachine where TState : State
{
    public TState CurrentState { get; private set; }

    public void Init(TState startingState)
    {
        CurrentState = startingState;
        startingState.EnterState();
    }

    public void ChangeState(TState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
    void IStateMachine.ChangeState(State newState)
    {
        ChangeState((TState)newState);
    }
}
