using System.Collections.Generic;
using UnityEngine;

public class CloseWeave : ActorEffect
{
    private CloseWeaveSO config;
    public CloseWeave(EffectTrigger triggerMask, EffectPriority priority, EffectStack stackType, CloseWeaveSO config) : base(triggerMask, priority, stackType)
    {
        this.config = config;
    }

    public override void OnTurnEnd(GridActor self)
    {
        base.OnTurnEnd(self);
        Debug.Log(string.Format("close weave {0}", self));
        List<GridActor> closeCell =
            GridPathfinder.FindAllActorsWithinRange(self.GetHoldingCell(), 1, MovementDirections.Cardinals, true);

        int alliesCount = 0;
        foreach (GridActor actor in closeCell)
        {
            if (actor is AbikuTrio trio)
            {
                alliesCount++;
            }
        }
        
        ActorEffectManager effectManager = self.GetComponent<ActorEffectManager>();
        if (effectManager != null)
        {
            DefenseBoostEffect newEffect = new DefenseBoostEffect(EffectTrigger.OnDamageMitigation, EffectPriority.First, EffectStack.oneMax, alliesCount);
            effectManager.AddEffect(newEffect);
        }
    }
}
