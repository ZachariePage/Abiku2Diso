using UnityEngine;

public abstract class HealingActorEffect : ActorEffect, IHealingSource
{
    public HealingActorEffect(EffectTrigger triggerMask, EffectPriority priority, EffectStack stackType = EffectStack.oneMax, int maxStacks = 1)
        : base(triggerMask, priority, stackType, maxStacks)
    {
    }

    public string DisplayName => GetType().Name;
    public GridActor SourceActor => effectApplier;
}
