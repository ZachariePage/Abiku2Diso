using UnityEngine;

public abstract class EnemyStance : State
{
    protected Enemy enemy;
    protected EnemyStance(GridActor unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
        enemy = unit as Enemy;
    }
    protected abstract void PerformAction();
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (enemy.IsCastingSpell())
        {
            EndTurn();
            return;
        }

        PerformAction();
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
    
    public abstract EnemyStanceScriptableObject GetStanceConfig();
}
