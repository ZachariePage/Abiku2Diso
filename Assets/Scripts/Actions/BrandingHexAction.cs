using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandingHexAction : DamagingAbility
{
    int normalAmount;
    int advantageAmount;
    public BrandingHexAction(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range, 
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element,
        BrandingHexTemplateSO config, int normalAmount, int advantageHexAmount)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.normalAmount = normalAmount;
        this.advantageAmount = advantageHexAmount;
    }
    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
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
        
        GridActor target = selectedTargets[0].GetActor();
        
        if (target != null)
        {
            bool advantage = ElementSystem.Instance.IsEffectiveAgainst(element.GetElementType(),
                target.GetComponent<IHoldElement>().GetElement());
            
            int amount;
            if (advantage)
            {
                amount = advantageAmount;
            }
            else
            {
                amount = normalAmount;
            }

            witch.ApplyHex(target, amount);

            DealDamageToTargets(selectedTargets, 10);
        }

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
