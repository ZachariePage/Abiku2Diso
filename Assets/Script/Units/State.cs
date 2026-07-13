using UnityEngine;

public class State
{
    protected GridActor unit;
    protected StateMachine stateMachine;

    public State(GridActor unit, StateMachine stateMachine)
    {
        this.unit = unit;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        StateScriptableObject config = stateMachine.CurrentEnemyState.GetConfig();
    }

    public virtual void StartTurn()
    {
        
    }

    public virtual void EndTurn()
    {
        
    }

    public virtual void ExitState()
    {
    }

    public virtual void FrameUpdate()
    {

    }
    public virtual void PhysicUpdate()
    {
    
    }
    

    public virtual StateScriptableObject GetConfig()
    {
        return null;
    }
}
