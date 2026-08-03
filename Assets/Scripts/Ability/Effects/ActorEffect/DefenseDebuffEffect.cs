using UnityEngine;

public class DefenseDebuffEffect : ActorEffect
{
    private int defenseDebuff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DefenseDebuffEffect(EffectTrigger triggerMask, EffectPriority priority, int defenseDebuff, EffectStack stackType = EffectStack.oneMax,
        int maxStacks = 1) : base(triggerMask, priority, stackType, maxStacks)
    {
        this.defenseDebuff = defenseDebuff;
    }

    public override void OnDamageMitigation(DamageMitigationContext ctx)
    {
        ctx.Defense -= Mathf.Max(0, CurrentStacks * defenseDebuff);
        ctx.Self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
