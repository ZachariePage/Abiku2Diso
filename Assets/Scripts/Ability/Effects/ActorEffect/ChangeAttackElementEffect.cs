using UnityEngine;

public class ChangeAttackElementEffect : ActorEffect
{
    private Element choseElement;

    public ChangeAttackElementEffect(Element choseElement, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 999)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.choseElement = choseElement;
        this.TriggerMask = EffectTrigger.OnDamageSend;
    }
    
    public override void OnApplication(GridActor self)
    {
        base.OnApplication(self);
    }

    public override void OnDamageSent(DamageProposalContext ctx)
    {
        base.OnDamageSent(ctx);
        ctx.DamageElement = choseElement;
        ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
