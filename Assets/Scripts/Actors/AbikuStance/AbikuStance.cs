using System;
using System.Collections.Generic;
using UnityEngine;

public enum AbikuStanceType
{
    None  = 0,
    Toucher  = 1 << 0,
    Vue = 1 << 1,
    Ouïe  = 1 << 2,
}

[Serializable]
public class AbikuStanceAbilityGroup
{
    public AbikuStanceType stance;
    public List<AbilityTemplateSO> abilities = new List<AbilityTemplateSO>();
}
public abstract class AbikuStance : State
{
    protected List<BattleAction> actions =  new List<BattleAction>();
    protected abstract AbikuStanceType GetStanceType();
    private AbikuTrio abiku;
    
    private MoveAbikuAction moveAbiku;
    private ChangeAbikuAction changeAbiku;

    private UsedActionTracker _coldownTracker;
    
    private bool _currentlyCasting = false;
    private bool _turnOver = false;
    private bool cheat_infiniteAbility;
    protected AbikuStance(GridActor unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
        if (unit is AbikuTrio trio)
        {
            abiku = trio;
        }
        else
        {
            Debug.LogError("This should never happen stance not built on abikutrio");
            return;
        }
          
        moveAbiku = new MoveAbikuAction(abiku, abiku);
        changeAbiku = new ChangeAbikuAction(abiku, abiku);
        
        actions.Add(moveAbiku);
        actions.Add(changeAbiku);
        
        Egungun egungun = abiku.GetEgungun();
        foreach (var ability in egungun.GetAbilitiesForStance(GetStanceType()))
        {
            actions.Add(ability.CreateAction(abiku, unit));
        }
        
        //events 
        abiku.onMyTurnStart += StartTurn;
        abiku.onMyTurnEnd += EndTurn;
        
        UnitSpawner.Instance.SpawnStanceMenu(this, abiku);

        _coldownTracker = new UsedActionTracker();
    }

    public IEnumerable<BattleAction> GetAbilities()
    {
        return actions;
    }

    public abstract AbikuStanceScriptableObject GetStanceConfig();
    

    public override void EnterState()
    {
        base.EnterState();
        abiku.SetElement(GetStanceConfig().element.GetElementType());
    }

    public override void StartTurn()
    {
        base.StartTurn();
    }

    public override void EndTurn()
    {
        _coldownTracker.ResetTurn();
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

    public void Cheat_ResetTurn()
    {
        _coldownTracker.ResetTurn();
    }

    public void Cheat_InfiniteAbilityUse()
    {
        cheat_infiniteAbility = !cheat_infiniteAbility;
        _coldownTracker.cheat_infinite = !_coldownTracker.cheat_infinite;
    }

    public bool Cheat_GetInfiniteAbility()
    {
        return cheat_infiniteAbility;
    }
}
