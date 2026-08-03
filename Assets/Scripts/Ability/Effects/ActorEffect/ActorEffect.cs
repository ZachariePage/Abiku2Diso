using System;
using UnityEngine;

[Flags]
public enum EffectTrigger
{
    OnTurnStart = 1 << 0,
    OnMove = 1 << 1,
    OnAbilityThrown = 1 << 2,
    OnAbilityFinished = 1 << 3,
    OnDamageMitigation= 1 << 4,
    OnDamageTaken = 1 << 5,
    OnDamageDealt = 1 << 6,
    OnTurnEnd = 1 << 7,
}

public enum EffectPriority
{
    First = 100,
    Second = 200,
    Third = 300,
    Fourth = 400,
}

public class RequestAbilityContext
{
    public AbilityAction requestedAbilityAction;
}
public class DamageMitigationContext
{
    public GridActor Self;
    public GridActor Source;
    public Element DamageElement;

    public float IncomingDamage;
    public int Defense;
    public bool Dodged;
}

public enum EffectStack
{
    oneMax,
    definedAmount,
    infinite,
    newInstance
}
[Serializable]
public class ActorEffect
{
    public string debugName;
    public EffectTrigger TriggerMask;
    public EffectPriority Priority;
    public EffectStack StackType = EffectStack.oneMax;
    public int MaxStacks = 1;
    public int CurrentStacks = 1;

    protected ActorEffect(EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 1)
    {
        debugName = GetType().Name;
        TriggerMask = triggerMask;
        Priority = priority;
        StackType = stackType;
        MaxStacks = maxStacks;
        CurrentStacks = 1;
    }
    public virtual object StackKey => GetType();

    public virtual void OnApplication(GridActor self)
    {
    }

    public virtual void OnTurnStart(GridActor self) { }
    public virtual void OnMove(GridActor self, GridCell from, GridCell to) { }
    public virtual void OnAbilityThrown(GridActor self, BattleAction ability) { }
    public virtual void OnAbilityFinished(GridActor self, AbilityAftermathInfo info) { }
    
    public virtual void OnDamageMitigation(DamageMitigationContext ctx) { }
    public virtual void OnDamageTaken(GridActor self, DamageInfo info) { }
    public virtual void OnDamageDealt(GridActor self, DamageInfo info) { }
    public virtual void OnTurnEnd(GridActor self) { }
    
    public virtual void OnRemoval(GridActor self)
    {
    }
}
