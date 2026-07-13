using UnityEngine;

public class FireState : State
{
    private FireStateSO config;
    private AbilityAction action;
    private Enemy enemy;
    private ITargettable currentTarget;
    public FireState(GridActor unit, StateMachine stateMachine, FireStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
        enemy = unit as Enemy;
        action = config.action.CreateAction(unit);
    }
    
    public override void EnterState()
    {
        base.EnterState();
        enemy.turnBeforeExecutingAction = 2;
        
        currentTarget = FindTarget();
        if (currentTarget != null)
        {
            action.AddTarget(currentTarget);
            currentTarget.AddHighlight(this, CellHighlightState.Targeted);
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (enemy.turnBeforeExecutingAction > 0)
        {
            enemy.turnBeforeExecutingAction--;
            return;
        }
        
        currentTarget.RemoveHighlight(this);
        enemy.ExecuteAction(action, OnActionFinished);
        enemy.ChangeStateThroughIncrementation();
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

    }

    public override void EndTurn()
    {
        base.EndTurn();
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
    public override StateScriptableObject GetConfig()
    {
        return config;
    }
}
