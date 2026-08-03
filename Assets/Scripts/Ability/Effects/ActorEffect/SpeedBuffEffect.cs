using UnityEngine;

public class SpeedBuffEffect : ActorEffect
{
    private float speedBuff;

    public SpeedBuffEffect(float speedBuff, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 999)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.speedBuff = speedBuff;
    }

    public override void OnApplication(GridActor self)
    {
        Debug.LogWarning("not implemented");
        base.OnApplication(self);
    }

    public override void OnRemoval(GridActor self)
    {
        Debug.LogWarning("not implemented");
        base.OnRemoval(self);
    }
}
