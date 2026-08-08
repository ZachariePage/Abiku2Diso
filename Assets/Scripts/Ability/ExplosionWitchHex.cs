using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionWitchHex : DamagingAbility
{
    private ExplosionWitchHexTemplateSO config;

    public ExplosionWitchHex(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element,
        ExplosionWitchHexTemplateSO config) : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.config = config;
    }
    
    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        if (actor is not AbikuTrio self ||
            self.GetEgungun() is not WitchHex witch)
        {
            Debug.LogError("Why is this spell not on a witch?");
            return null;
        }
        
        IEnumerable<GridCell> reachable = direction.FindCellsWithinRange(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, targetAllowed.allowedTarget, true);
        
        List<ITargettable> validTargets = new List<ITargettable>();
        foreach (GridCell cell in reachable)
        {
            GridActor actor = cell.GetActorOnCell();
            if (actor == null) continue;
            
            if (witch.GetHexedActors().Contains(actor))
            {
                validTargets.Add(actor);
                validTargets.Add(cell);
            }
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
        GridActor target = selectedTargets[0].GetActor();

        if (target == null || actor is not AbikuTrio self || self.GetEgungun() is not WitchHex witch)
        {
            onComplete?.Invoke();
            yield break;
        }

        int available = witch.GetHexCount(target);
        int consumed = witch.ConsumeHex(target, available); 
        int radius = config.RangeExplosion + consumed * config.RangeExplosionPerHex; 

        List<GridActor> victims = GridPathfinder.FindAllActorsWithinRange(target.GetHoldingCell(), radius, direction.allowedDirections);
        victims.Add(selectedTargets[0].GetActor());
        
        if (!config.HurtAllies)
        {
            victims.RemoveAll(affected => affected.GetMyTeam() != Team.enemies);
        }

        DealDamageToTargets(victims, config.damage + config.damagePerHex * consumed);

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
