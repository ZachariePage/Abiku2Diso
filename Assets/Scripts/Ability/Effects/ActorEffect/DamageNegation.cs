using UnityEngine;

public class DamageNegation : ActorEffect
{
    public DamageNegation(EffectTrigger triggerMask, EffectPriority priority, EffectStack stackType = EffectStack.infinite,
        int maxStacks = 1) : base(triggerMask, priority, stackType, maxStacks)
    {
    }
    
    public override void OnApplication(GridActor self)
    {
        base.OnApplication(self);
    }

    public override void OnDamageMitigation(DamageMitigationContext ctx)
    {
        ctx.IncomingDamage = 0;
        CurrentStacks--;
        if (CurrentStacks <= 0)
        {
            ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
        }
    }
}
