using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Reversal : AbilityAction
{
    int negationAmount;
    int healAmount;
    public Reversal(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range, 
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element,
        ReversalTemplateSO config, int reduceDamagePercentage, int healPercentage)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.negationAmount = reduceDamagePercentage;
        this.healAmount = healPercentage;
    }
    public override TargetMode TargetMode()
    {
        return global::TargetMode.Single;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        IEnumerable<GridCell> reachable = direction.FindCellsWithinRange(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, targetAllowed.allowedTarget, true);
        
        List<ITargettable> validTargets = new List<ITargettable>();
        foreach (GridCell cell in reachable)
        {
            GridActor actor = cell.GetActorOnCell();
            if (actor != null)
            {
                validTargets.Add(actor);
            }
            
            validTargets.Add(cell);
        }
        
        return validTargets;
    }
    
    public override IEnumerable<GridCell> GetReachableCells()
    {
        IEnumerable<GridCell> reachable = direction.GetReachableCells(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, true);
        return reachable;
    }

    public override bool TryExecute(ITargettable target)
    {
        throw new System.NotImplementedException();
    }
    
    public override IEnumerator Execute(Action onComplete)
    {
        if (actor is not AbikuTrio self ||
            self.GetEgungun() is not WitchHex witch)
        {
            Debug.LogError("Why is this spell not on a witch?");
            onComplete?.Invoke();
            yield break;
        }

        int hexAmount = math.min(witch.GetHexCount(actor), 3);
        if(hexAmount <= 0)
        {
            onComplete?.Invoke();
            yield break;
        }
        
        GridActor target = selectedTargets[0].GetActor();
        ActorEffectManager manager = target.GetComponent<ActorEffectManager>();

        if (manager == null)
        {
            onComplete?.Invoke();
            yield break;
        }
        
        switch (hexAmount)
        {
            case 1:
                DamageMitigationEffect mitigationEffect = new DamageMitigationEffect(negationAmount,
                    EffectTrigger.OnDamageMitigation, EffectPriority.First, EffectStack.oneMax, 1);
                manager.AddEffect(mitigationEffect, actor);
                break;
            case 2:
                DamageNegation negationEffect = new DamageNegation(EffectTrigger.OnDamageMitigation, EffectPriority.First);
                manager.AddEffect(negationEffect, actor);
                break;
            case 3:
                NegateToHeal negateToHeal = new NegateToHeal(healAmount, EffectTrigger.OnDamageMitigation, EffectPriority.First);
                manager.AddEffect(negateToHeal, actor);
                break;
        }

        witch.ConsumeHex(actor, hexAmount);
        onComplete?.Invoke();
        yield return null;
    }
    
    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.MoveRange;
    }

    public override bool IsReady()
    {
        if (selectedTargets.Count >= numberOfTargets)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool AddTarget(ITargettable target)
    {
        selectedTargets.Clear();
        selectedTargets.Add(target);
        return true;
    }
}
