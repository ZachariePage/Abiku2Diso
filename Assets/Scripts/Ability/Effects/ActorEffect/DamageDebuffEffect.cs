using UnityEngine;

public class DamageDebuffEffect : ActorEffect
{
    private int damageDebuff;
    private bool toRemoveAtEndTurn = false;
    public DamageDebuffEffect(int damageDebuff, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 999)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.damageDebuff = damageDebuff;
        this.TriggerMask = EffectTrigger.OnDamageSend | EffectTrigger.OnTurnEnd;
    }
    public override void OnDamageSent(DamageProposalContext ctx)
    {
        base.OnDamageSent(ctx);
        ctx.DamageProposed += damageDebuff * CurrentStacks;
        toRemoveAtEndTurn =  true;
    }

    public override void OnTurnEnd(GridActor self)
    {
        base.OnTurnEnd(self);
        if (toRemoveAtEndTurn)
        {
            self.GetComponent<ActorEffectManager>().RemoveEffect(this);
        }
    }
    
}
