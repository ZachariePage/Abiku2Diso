using UnityEngine;

public class SpeedDebuffEffect : ActorEffect
{
    private float speedDebuff;
    public SpeedDebuffEffect(int speedDebuff, EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 999)
        : base(triggerMask, priority, stackType, maxStacks)
    {
        this.speedDebuff = speedDebuff;
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
