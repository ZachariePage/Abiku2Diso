using UnityEngine;

public class DamageBuffEffect : ActorEffect
{
    private int damageBuff;
    private bool toRemoveAtEndTurn = false;
    public DamageBuffEffect(int damageBuff, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.infinite, int maxStacks = 999)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.damageBuff = damageBuff;
        this.TriggerMask = EffectTrigger.OnDamageSend | EffectTrigger.OnTurnEnd;
    }

    public override void OnDamageSent(DamageProposalContext ctx)
    {
        base.OnDamageSent(ctx);
        ctx.DamageProposed += damageBuff * CurrentStacks;
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
