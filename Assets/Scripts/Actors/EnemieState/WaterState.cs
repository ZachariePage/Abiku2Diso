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
        action = config.action.CreateAction(unit);
    }
    
    public override void EnterState()
    {
        base.EnterState();
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

    public override void EndTurn()
    {
        base.EndTurn();
    }
    
    private void OnActionFinished()
    {

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
