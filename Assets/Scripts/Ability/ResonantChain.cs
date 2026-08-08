using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResonantChain : DamagingAbility
{
    private ResonantChainTemplateSO config;

    public ResonantChain(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range, TargetingStrategySO direction,
        TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, ResonantChainTemplateSO config, float damage)
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
        
        List<GridActor> targetToDamage = new List<GridActor>();
        GridActor[] targets = witch.GetHexedActors().ToArray();

        foreach (GridActor target in targets)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                targetToDamage.Add(target);
                witch.ConsumeHex(target, 1);
            }
        }

        DealDamageToTargets(targetToDamage, config.damage);
        
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
        return true;
    }
}
