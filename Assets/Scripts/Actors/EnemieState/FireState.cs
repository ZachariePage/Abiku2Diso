using UnityEngine;

public class FireState : State
{
    private FireStateSO config;
    private AbilityAction action;
    private Enemy enemy;
    private ITargettable currentTarget;
    public FireState(GridActor unit, IStateMachine stateMachine, FireStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
        enemy = unit as Enemy;
        action = config.action.CreateAction(enemy, unit);
        
    }
    
    public override void EnterState()
    {
        base.EnterState();
        enemy.turnBeforeExecutingAction = 1;
        enemy.SetElement(config.element.GetElementType());
        currentTarget = FindTarget();
        if (currentTarget != null)
        {
            Debug.Log(currentTarget);
            action.AddTarget(currentTarget);
            currentTarget.AddHighlight(this, CellHighlightState.Targeted);
        }
        else
        {
            Debug.Log("No target found");
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (enemy.turnBeforeExecutingAction > 0)
        {
            enemy.turnBeforeExecutingAction--;
            EndTurn();
            return;
        }

        if (currentTarget != null)
        {
            currentTarget.RemoveHighlight(this);
        }
        else
        {
            Debug.Log("No target found");
        }
        
        Debug.Log("execute action ");
        enemy.ExecuteAction(action, OnActionFinished);
    }
    
    ITargettable FindTarget()
    {
        ITargettable idealTarget = null;
        float minDistance = float.MaxValue;
        foreach (var target in action.GetValidTargets())
        {
            float distance = Vector3.Distance(unit.transform.position, target.GetWorldPosition());
            if (distance < minDistance)
            {
                minDistance = distance;
                idealTarget = target;
            }
        }
        
        return idealTarget;
    }

    private void OnActionFinished()
    {
        Debug.Log("Enemy fireState action finished");
        enemy.ChangeStateThroughIncrementation();
        EndTurn();
    }

    public override void EndTurn()
    {
        Debug.Log("internal endturn");
        base.EndTurn();
        enemy.EndTurn();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    
}
