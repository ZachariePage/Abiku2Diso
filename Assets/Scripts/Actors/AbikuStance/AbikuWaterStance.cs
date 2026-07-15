using UnityEngine;

public class AbikuWaterStance : AbikuStance
{
    private AbikuWaterStanceSO config;


    public AbikuWaterStance(GridActor unit, StateMachine stateMachine, AbikuWaterStanceSO config) : base(unit, stateMachine)
    {
        this.config = config;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void StartTurn()
    {
        base.StartTurn();
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

    

    protected override AbikuStanceType GetStanceType()
    {
        return AbikuStanceType.Water;
    }
}
