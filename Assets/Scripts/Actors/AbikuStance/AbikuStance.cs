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
public abstract class AbikuStance : State
{
    protected List<BattleAction> actions =  new List<BattleAction>();
    protected abstract AbikuStanceType GetStanceType();
    private AbikuTrio abiku;
    
    private MoveAbikuAction moveAbiku;
    private ChangeAbikuAction changeAbiku;

    protected AbikuStance(GridActor unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
        if (unit is AbikuTrio trio)
        {
            abiku =  trio;
        }
        else
        {
            Debug.LogError("This should never happen stance not built on abikutrio");
            return;
        }
          
        moveAbiku = new MoveAbikuAction(abiku);
        changeAbiku = new ChangeAbikuAction(abiku);
        
        actions.Add(moveAbiku);
        actions.Add(changeAbiku);
        
        Egungun egungun = abiku.GetEgungun();
        foreach (var ability in egungun.GetAbilitiesForStance(GetStanceType()))
        {
            actions.Add(ability.CreateAction(unit));
        }

        
        UnitSpawner.Instance.SpawnStanceMenu(this, abiku);
    }

    public IEnumerable<BattleAction> GetAbilities()
    {
        return actions;
    }
}
