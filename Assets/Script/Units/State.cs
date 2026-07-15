using UnityEngine;

public class State
{
    protected GridActor unit;
    protected IStateMachine stateMachine;

    protected State(GridActor unit, IStateMachine stateMachine)
    {
        this.unit = unit;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        
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
    

    
}
