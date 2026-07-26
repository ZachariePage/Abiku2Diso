using UnityEngine;
using UnityEngine.Assertions;

public class WaterState : EnemyStance
{
    private WaterStanceSO config;
    private AbilityAction action;
    private Enemy enemy;
    
    private ITargettable currentTarget;
    public WaterState(GridActor unit, IStateMachine stateMachine, WaterStanceSO config) : base(unit, stateMachine)
    {
        this.config = config;
        enemy = unit as Enemy;
        action = config.action.CreateAction(enemy, unit);
        
    }

    protected override void PerformAction()
    {
        enemy.ExecuteAction(action, OnActionFinished);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.SetElement(config.element.GetElementType());
        
        ITargettable target = FindTarget();
        if (target != null)
        {
            action.AddTarget(target);
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
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
        EndTurn();
    }

    public override void EndTurn()
    {
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

    public override EnemyStanceScriptableObject GetStanceConfig()
    {
        return config;
    }
}
