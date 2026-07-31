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
    private bool currentlyCasting = false;
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
        abiku.onMyTurnEnd += EndTurn;
        
        UnitSpawner.Instance.SpawnStanceMenu(this, abiku);

        _coldownTracker = new UsedActionTracker();
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

    public bool IsCastingSpell()
    {
        return currentlyCasting;
    }

    public void SetCastingSpell(bool value, CastingSpellColdownType type)
    {
        switch (type)
        {
            case CastingSpellColdownType.enemy:
                break;
            case CastingSpellColdownType.player:
                currentlyCasting = value;
                break;
            case CastingSpellColdownType.both:
                currentlyCasting = value;
                break;
        }
    }

    public void OnAbilityThrown()
    {
        
    }

    public void OnAbilityFinished(AbilityAftermathInfo abilityAftermathInfo)
    {
        abiku.OnAbilityFinished(abilityAftermathInfo);
    }

    public GridActor GetActor()
    {
        return abiku;
    }

    public UsedActionTracker GetCooldownTracker()
    {
        return _coldownTracker;
    }

    public override void EnterState()
    {
        base.EnterState();
        abiku.SetElement(GetStanceConfig().element.GetElementType());
    }

    public override void StartTurn()
    {
        RefreshColdown();
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
}
