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
    protected List<AbilityAction> actions;
    protected abstract AbikuStanceType GetStanceType();

    protected AbikuStance(GridActor unit, StateMachine stateMachine) : base(unit, stateMachine)
    {
        if (unit is AbikuTrio trio)
        {
            Egungun egungun = trio.GetEgungun();
            foreach (var ability in egungun.GetAbilitiesForStance(GetStanceType()))
            {
                actions.Add(ability.CreateAction(unit));
            }
        }
        else
        {
            Debug.LogError("This should never happen stance not built on abikutrio");
        }

    }
}
