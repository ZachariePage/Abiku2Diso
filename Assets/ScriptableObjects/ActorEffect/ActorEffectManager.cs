using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorEffectManager : MonoBehaviour
{
    private GridActor _self;
    [SerializeField] private List<ActorEffectDefinition> _startingEffectDefinitions;

    [SerializeReference] private List<ActorEffect> _activeEffects = new();

    private void Awake()
    {
        _self = GetComponent<GridActor>();
        foreach (var def in _startingEffectDefinitions)
        {
            AddEffect(def, _self);
        }
    }

    public void AddEffect(ActorEffectDefinition definition, GridActor effectApplier)
    {
        ActorEffect effect = definition.CreateEffect();
        AddEffect(effect, _self);
    }

    //will cause a bug if the effect is for exemple oneStack but then we try to apply same effect but as another stack type
    public void AddEffect(ActorEffect newEffect, GridActor effectApplier)
    {
        ActorEffect existing = _activeEffects.FirstOrDefault(e => Equals(e.EffectKey, newEffect.EffectKey));
        if (existing == null)
        {
            _activeEffects.Add(newEffect);
            newEffect.managerOwner = _self;
            newEffect.effectApplier = effectApplier;
            newEffect.OnApplication(_self);
            return;
        }

        switch (existing.StackType)
        {
            case EffectStack.oneMax:
                break;

            case EffectStack.definedAmount:
                if (existing.CurrentStacks < existing.MaxStacks)
                {
                    existing.CurrentStacks++;
                }
                break;

            case EffectStack.infinite:
                existing.CurrentStacks++;
                break;
            case EffectStack.newInstance:
                _activeEffects.Add(newEffect);
                newEffect.managerOwner = _self;
                newEffect.effectApplier = effectApplier;
                newEffect.OnApplication(_self);
                break;
        }
    }

    public void RemoveEffect(ActorEffect effect)
    {
        effect.OnRemoval(_self);
        _activeEffects.Remove(effect);
    }

    public void RemoveStack(ActorEffect effect)
    {
        if (effect.CurrentStacks > 1)
        {
            effect.CurrentStacks--;
        }
        else
        {
            _activeEffects.Remove(effect);
        }
    }
    
    public T GetEffect<T>() where T : ActorEffect
    { 
        foreach (ActorEffect effect in _activeEffects)
        {
            if (effect is T typedEffect)
            {
                return typedEffect;
            }
        }

        return null;
    }

    public bool HasEffect<T>() where T : ActorEffect
    {
        return GetEffect<T>() != null;
    }
    
    public void TriggerOnTurnStart(GridActor self) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnTurnStart)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnTurnStart(_self);
        }
    }
    public void TriggerOnMove(GridActor self, GridCell from, GridCell to) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnMove)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnMove(_self, from, to);
        }
    }
    public void TriggerOnAbilityThrown(GridActor self, BattleAction ability) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnAbilityThrown)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnAbilityThrown(_self, ability);
        }
    }
    public void TriggerOnAbilityFinished(GridActor self, AbilityAftermathInfo info) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnAbilityFinished)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnAbilityFinished(_self, info);
        }
    }

    public void TriggerOutgoingDamage(DamageProposalContext ctx)
    {
        foreach (var effect in GetEffectsFor(EffectTrigger.OnDamageSend))
        {
            effect.OnDamageSent(ctx);
        }
    }
    public void TriggerDamageMitigation(DamageMitigationContext ctx)
    {
        foreach (var effect in GetEffectsFor(EffectTrigger.OnDamageMitigation))
        {
            effect.OnDamageMitigation(ctx);
        }
    }
    
    public void TriggerOnDamageTaken(GridActor self, DamageInfo info) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnDamageTaken)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnDamageTaken(_self, info);
        }
    }
    public void TriggerOnDamageDealt(GridActor self, DamageInfo info) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnDamageDealt)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnDamageDealt(_self, info);
        }
    }
    public void TriggerOnTurnEnd(GridActor self) 
    {
        foreach (var e in _activeEffects.Where(e => (e.TriggerMask & EffectTrigger.OnTurnEnd)
                                                    != 0).OrderBy(e => e.Priority).ToList())
        {
            e.OnTurnEnd(_self);
        }
    }
    
    //to switch to this method when im not lazy to do it
    private List<ActorEffect> GetEffectsFor(EffectTrigger trigger)
    {
        return _activeEffects
            .Where(e => (e.TriggerMask & trigger) != 0)
            .OrderBy(e => e.Priority)
            .ToList();
    }
}
