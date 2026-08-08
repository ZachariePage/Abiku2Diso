using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapHex : AbilityAction
{
    private SnapHexTemplate config;
    public SnapHex(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, SnapHexTemplate config)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.config = config;
    }
    
    public override TargetMode TargetMode()
    {
        return global::TargetMode.Instant;
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

        foreach (ITargettable target in GetValidTargets())
        {
            IDamageable damageable = target.GetActor().GetComponent<IDamageable>();

            if (damageable != null && damageable.IsWounded())
            {
                witch.ApplyHex(target.GetActor(), config.numberOfHexesPerTarget);
            }
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
        return true;
    }

    public override bool AddTarget(ITargettable target)
    {
        selectedTargets.Clear();
        selectedTargets.Add(target);
        return true;
    }
}
