using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingWall : AbilityAction
{
    private StandingWallTemplateSO config;

    public StandingWall(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, StandingWallTemplateSO config)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.config = config;
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
            self.GetEgungun() is not ShieldBreaker shieldBreaker)
        {
            Debug.LogError("Why is this spell not on a shieldBreaker?");
            onComplete?.Invoke();
            yield break;
        }

        if (selectedTargets[0].GetTargetCell().GetActorOnCell() != null)
        {
            Debug.Log("meow");
        }
        //shieldBreaker.BoulderManager.Summon(config.typeToSpawn, selectedTargets[0].GetTargetCell());
        
        onComplete?.Invoke();
        yield return null;
    }
    
    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.MoveRange;
    }

    public override bool IsReady()
    {
        if (actor is not AbikuTrio self ||
            self.GetEgungun() is not ShieldBreaker shieldBreaker)
        {
            Debug.LogError("Why is this spell not on a shieldBreaker?");
            return false;
        }
        
        if (!shieldBreaker.BoulderManager.CanSpawnTypeOfBoulder(config.typeToSpawn))
        {
            return false;     
        }
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
