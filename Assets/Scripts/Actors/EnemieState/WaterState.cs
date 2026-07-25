using UnityEngine;

public class WaterState : State
{
    private WaterStanceSO config;
    private AbilityAction action;
    private Enemy enemy;
    
    public WaterState(GridActor unit, IStateMachine stateMachine, WaterStanceSO config) : base(unit, stateMachine)
    {
        this.config = config;
        enemy = unit as Enemy;
        action = config.action.CreateAction(enemy, unit);
        
    }
    
    public override void EnterState()
    {
        base.EnterState();
        enemy.SetElement(config.element.GetElementType());
    }

    public override void StartTurn()
    {
        base.StartTurn();
        
        ITargettable target = FindTarget();
        if (target != null)
        {
            action.AddTarget(target);
        }
        
        enemy.ExecuteAction(action, OnActionFinished);
    }
    
    ITargettable FindTarget()
    {
        ITargettable idealTarget = null;
        float minDistance = float.MinValue;
        foreach (var target in action.GetValidTargets())
        {
            float distance = Vector3.Distance(unit.transform.position, target.GetWorldPosition());
            if (distance > minDistance)
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
