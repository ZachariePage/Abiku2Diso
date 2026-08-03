using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodInscription : AbilityAction
{
    public BloodInscription(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, BloodInscriptionTemplate config)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Instant;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        return null;
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
        witch.ApplyHex(self, 3);
        self.GetComponent<ActorEffectManager>().AddEffect(new BloodInscriptionEffect(3));
        
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
