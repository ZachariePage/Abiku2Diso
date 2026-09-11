using System;
using UnityEngine;

[Flags]
public enum EffectTrigger
{
    none = 0,
    OnTurnStart = 1 << 0,
    OnMove = 1 << 1,
    OnAbilityThrown = 1 << 2,
    OnAbilityFinished = 1 << 3,
    OnDamageSend = 1 << 4,
    OnDamageMitigation= 1 << 5,
    OnDamageTaken = 1 << 6,
    OnDamageDealt = 1 << 7,
    OnTurnEnd = 1 << 8,
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
    
    public GridActor managerOwner;
    public GridActor effectApplier;

    //I have realised that this is very bad, i realized that basically designer should never be allowed to decide the activation
    //so constructor don't need effecttrigger and potentially also effect stack, just confuse other prog that might join this project
    //but anyway i am alone and nobody will ever read this so xDDDDDDDDDDDDDDDDD
    public ActorEffect(EffectTrigger triggerMask, EffectPriority priority,
        EffectStack stackType = EffectStack.oneMax, int maxStacks = 999)
    {
        debugName = GetType().Name;
        TriggerMask = triggerMask;
        Priority = priority;
        StackType = stackType;
        MaxStacks = maxStacks;
        CurrentStacks = 1;
    }
    public virtual object EffectKey => GetType();

    public virtual void OnApplication(GridActor self)
    {
    }

    public virtual void OnTurnStart(GridActor self) { }
    public virtual void OnMove(GridActor self, GridCell from, GridCell to) { }
    public virtual void OnAbilityThrown(GridActor self, BattleAction ability) { }
    public virtual void OnAbilityFinished(GridActor self, AbilityAftermathInfo info) { }
    public virtual void OnDamageSent(DamageProposalContext ctx) { }
    public virtual void OnDamageMitigation(DamageMitigationContext ctx) { }
    public virtual void OnDamageTaken(GridActor self, DamageInfo info) { }
    public virtual void OnDamageDealt(GridActor self, DamageInfo info) { }
    public virtual void OnTurnEnd(GridActor self) { }
    public virtual void OnRemoval(GridActor self) { }
}
