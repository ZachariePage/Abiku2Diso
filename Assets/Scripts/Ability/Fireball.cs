using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AbilityAction
{
    
    private List<GameCue> onAbilityThrownCues = new List<GameCue>();
    public Fireball(ISpellCaster caster, GridActor actor, int range, TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, GameCue[] AbilityThrownCues)
        : base(caster, actor, range, direction, targetAllowed, numberOfTargets, element)
    {
        foreach (var cue in AbilityThrownCues)
        {
            onAbilityThrownCues.Add(cue);
        }
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        //List<GridCell> reachable = GridPathfinder.FindCellsWithinRange(actor.GetHoldingCell(), range, targetAllowed, direction, true);
        //List<GridCell> reachable = GridPathfinder.GetReachableCellsRay(actor.GetHoldingCell(), range, direction.allowedDirections, true);
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
        //List<GridCell> reachable = GridPathfinder.GetReachableCells(actor.GetHoldingCell(), range, direction, true);
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
        foreach (GameCue cue in onAbilityThrownCues)
        {
            cue?.Execute(actor.GetWorldPosition());
        }
        yield return new WaitForSeconds(2f);
        DealDamageToTargets(selectedTargets, 10);
        yield return new WaitForSeconds(2f);
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
