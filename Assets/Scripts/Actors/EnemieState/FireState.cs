using UnityEngine;

public class FireState : EnemyStance
{
    private FireStateSO config;
    private AbilityAction action;
    private ITargettable currentTarget;
    public FireState(GridActor unit, IStateMachine stateMachine, FireStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
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
        currentTarget = FindTarget();
        if (currentTarget != null)
        {
            Debug.Log(currentTarget);
            action.AddTarget(currentTarget);
        }
        else
        {
            Debug.Log("No target found");
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
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
