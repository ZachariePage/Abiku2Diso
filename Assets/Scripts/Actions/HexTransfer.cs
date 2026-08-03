using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexTransfer : AbilityAction
{
    public HexTransfer(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, HexTransferTemplateSO config)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        List<ITargettable> validTargets = new List<ITargettable>();
        if (actor is not AbikuTrio self ||
            self.GetEgungun() is not WitchHex witch)
        {
            return validTargets;
        }
        
        IEnumerable<GridCell> reachable = direction.FindCellsWithinRange(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, targetAllowed.allowedTarget, true);
        
        foreach (GridCell cell in reachable)
        {
            GridActor actor = cell.GetActorOnCell();
            if (actor != null)
            {
                if (witch.IsHexingTarget(actor))
                {
                    validTargets.Add(actor);
                }
            }
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

        GridActor firstTarget = selectedTargets[0].GetActor();
        GridActor secondTarget = selectedTargets[1].GetActor();

        GridCell firstCell = firstTarget.GetHoldingCell();
        GridCell secondCell = secondTarget.GetHoldingCell();
        
        witch.ApplyHex(firstTarget, 1);
        witch.ApplyHex(secondTarget, 1);
        
        firstTarget.MoveToCell(secondCell);
        secondTarget.MoveToCell(firstCell);
        
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
        selectedTargets.Add(target);
        return true;
    }
}
