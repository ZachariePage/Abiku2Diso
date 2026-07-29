using UnityEngine;

public class CrossStep : ActorEffect
{
    private CrossStepEffectSO config;
    public CrossStep(EffectTrigger triggerMask, EffectPriority priority, CrossStepEffectSO config,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 1)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.config = config;
    }

    public override void OnTurnStart(GridActor self)
    {
        base.OnTurnStart(self);
        self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
