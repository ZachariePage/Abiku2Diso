using UnityEngine;

public class DamageMitigationEffect : ActorEffect
{
    private int percentageNegated;
    
    public DamageMitigationEffect(int percentageNegated, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 1)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.percentageNegated = percentageNegated;
    }

    public override void OnApplication(GridActor self)
    {
        base.OnApplication(self);
        if (percentageNegated <= 0)
        {
            self.GetComponent<ActorEffectManager>().RemoveEffect(this);
        }
    }

    public override void OnDamageMitigation(DamageMitigationContext ctx)
    {
        ctx.IncomingDamage *= (percentageNegated / 100);
        ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
