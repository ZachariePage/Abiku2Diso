using UnityEngine;

public class NegateToHeal : ActorEffect
{
    private int percentageHealed;

    public NegateToHeal(int percentageNegated, EffectTrigger triggerMask, EffectPriority priority, 
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 1)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.percentageHealed = percentageNegated;
    }
    
    public override void OnDamageMitigation(DamageMitigationContext ctx)
    {
        float healing = ctx.IncomingDamage;
        healing *= (percentageHealed / 100);
        ctx.IncomingDamage = 0;
        ctx.Self.GetComponent<IDamageable>().Heal(ctx.Self, null, healing, Element.None);
        ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
