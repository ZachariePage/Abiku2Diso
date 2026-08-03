using UnityEngine;

public class DefenseBoostEffect : ActorEffect
{
    private int defenseBoost;

    public DefenseBoostEffect(EffectTrigger triggerMask, EffectPriority priority, EffectStack stackType, int defenseBoost) : base(triggerMask,
        priority, stackType)
    {
        this.defenseBoost = defenseBoost;
    }

    public override void OnDamageMitigation(DamageMitigationContext ctx)
    {
        ctx.Defense += CurrentStacks * defenseBoost;
        ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
