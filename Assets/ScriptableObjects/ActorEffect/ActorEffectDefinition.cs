using UnityEngine;

public abstract class ActorEffectDefinition : ScriptableObject
{
    [Header("Display")]
    public string EffectName;

    public EffectTrigger TriggerMask;
    public EffectPriority Priority = EffectPriority.First;
    public EffectStack StackType = EffectStack.oneMax;
    
    public int StackCount = 1;

    public abstract ActorEffect CreateEffect();
}
