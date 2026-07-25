using System.Collections.Generic;
using UnityEngine;

public class AbikuFireStance : AbikuStance
{
    private AbikuFireStanceSO config;
    private List<AbilityAction> actions = new List<AbilityAction>();


    public AbikuFireStance(GridActor unit, IStateMachine stateMachine, AbikuFireStanceSO config) : base(unit, stateMachine)
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
        return AbikuStanceType.Fire;
    }

    public override AbikuStanceScriptableObject GetStanceConfig()
    {
        return config;
    }
}
