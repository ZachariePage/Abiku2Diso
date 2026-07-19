using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AbilityAction
{
    
    private List<GameCue> onAbilityThrownCues = new List<GameCue>();
    public Fireball(GridActor actor, int range, MovementDirections direction, TargetType targetAllowed, int numberOfTargets, ElementSO element, GameCue[] AbilityThrownCues)
        : base(actor, range, direction, targetAllowed, numberOfTargets, element)
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
        List<GridCell> reachable = GridPathfinder.FindCellsWithinRange(actor.GetHoldingCell(), range, targetAllowed, direction, true);
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
        List<GridCell> reachable = GridPathfinder.GetReachableCells(actor.GetHoldingCell(), range, direction, true);
        
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
