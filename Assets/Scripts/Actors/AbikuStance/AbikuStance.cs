using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum AbikuStanceType
{
    None  = 0,
    Fire  = 1 << 0,
    Water = 1 << 1,
    Wood  = 1 << 2,
}

[Serializable]
public class AbikuStanceAbilityGroup
{
    public AbikuStanceType stance;
    public List<AbilityTemplateSO> abilities = new List<AbilityTemplateSO>();
}
public abstract class AbikuStance : State, ISpellCaster
{
    protected List<BattleAction> actions =  new List<BattleAction>();
    protected abstract AbikuStanceType GetStanceType();
    private AbikuTrio abiku;
    
    private MoveAbikuAction moveAbiku;
    private ChangeAbikuAction changeAbiku;

    private UsedActionTracker _coldownTracker;
    
    private bool hasPlayedThisTurn = false;
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
          
        moveAbiku = new MoveAbikuAction(this, abiku);
        changeAbiku = new ChangeAbikuAction(this, abiku);
        
        actions.Add(moveAbiku);
        actions.Add(changeAbiku);
        
        Egungun egungun = abiku.GetEgungun();
        foreach (var ability in egungun.GetAbilitiesForStance(GetStanceType()))
        {
            actions.Add(ability.CreateAction(this, unit));
        }
        
        //events 
        abiku.onMyTurnStart += StartTurn;
        
        UnitSpawner.Instance.SpawnStanceMenu(this, abiku);
    }

    public IEnumerable<BattleAction> GetAbilities()
    {
        return actions;
    }

    public abstract AbikuStanceScriptableObject GetStanceConfig();

    public void PutAbilityOnColdown()
    {
        hasPlayedThisTurn = true;
    }

    public void RefreshColdown()
    {
        hasPlayedThisTurn = false;
    }

    public bool IsOnColdown()
    {
        return hasPlayedThisTurn;
    }

    public bool CanThrowSpell()
    {
        return !abiku.IsOnLastStance();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void StartTurn()
    {
        RefreshColdown();
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
}
