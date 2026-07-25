using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAttack : AbilityAction
{
    private DelayedExplosionEffect attack;
    protected GridCell targetedCell;


    public DelayedAttack(ISpellCaster caster, GridActor actor, int range, TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
        : base(caster,actor, range, direction, targetAllowed, numberOfTargets, element)
    {
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        //List<GridCell> reachable = GridPathfinder.FindCellsWithinRange(actor.GetHoldingCell(), range, targetAllowed.allowedTarget, direction.allowedDirections, true);
        IEnumerable<GridCell> reachable = direction.FindCellsWithinRange(actor.GetHoldingCell(), range,
            direction.allowedDirections, direction.directionType, targetAllowed.allowedTarget, true);
        return reachable;
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
        onComplete?.Invoke();
        attack = new DelayedExplosionEffect(targetedCell, 3, 2, 3, MovementDirections.Cardinals, element);
        yield return null;
    }

    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.MoveRange;
    }

    public override bool IsReady()
    {
        if (targetedCell != null)
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
        targetedCell = target as GridCell;
        return true;
    }
}
