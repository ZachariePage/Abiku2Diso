using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Omen : AbilityAction
{
    private OmenTemplateSO config;
    public Omen(OmenTemplateSO config, ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range, TargetingStrategySO direction,
        TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.config = config;
    }
    
    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        IEnumerable<GridCell> reachable = direction.FindCellsWithinRange(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, targetAllowed.allowedTarget, true);
        
        List<ITargettable> validTargets = new List<ITargettable>();
        foreach (GridCell cell in reachable)
        {
            GridActor actor = cell.GetActorOnCell();
            if (actor != null)
            {
                validTargets.Add(actor);
            }
            
            validTargets.Add(cell);
        }
        
        return validTargets;
    }
    
    public override IEnumerable<GridCell> GetReachableCells()
    {
        IEnumerable<GridCell> reachable = direction.GetReachableCells(actor.GetHoldingCell(), range, 
            direction.allowedDirections, direction.directionType, true);
        return reachable;
    }

    public override bool TryExecute(ITargettable target)
    {
        throw new System.NotImplementedException();
    }
    
    public override IEnumerator Execute(Action onComplete)
    {
        if (actor is not AbikuTrio self ||
            self.GetEgungun() is not WitchHex witch)
        {
            Debug.LogError("Why is this spell not on a witch?");
            onComplete?.Invoke();
            yield break;
        }
        int hexAmount = math.min(witch.GetHexCount(actor), 3);
        if(hexAmount <= 0)
        {
            onComplete?.Invoke();
            yield break;
        }
        
        GridActor target = selectedTargets[0].GetActor();
        ActorEffectManager manager = target.GetComponent<ActorEffectManager>();

        if (manager == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        ActorEffect effect;
        switch (target.GetMyTeam())
        {
            case Team.allies:
                switch (hexAmount)
                {
                    case 1:
                        effect = new SpeedBuffEffect(config.speedBuffFlatAmount, EffectTrigger.none,
                            EffectPriority.First, EffectStack.infinite);
                        manager.AddEffect(effect, actor);
                        break;
                    case 2:
                        effect = new DefenseBoostEffect(EffectTrigger.OnDamageMitigation, EffectPriority.First,
                            EffectStack.infinite, config.defenseBuffFlatAmount);
                        manager.AddEffect(effect, actor);
                        break;
                    case 3:
                        effect = new DamageBuffEffect(config.damageBuffFlatAmount,
                            EffectTrigger.OnDamageSend | EffectTrigger.OnTurnEnd, EffectPriority.First,
                            EffectStack.infinite);
                        manager.AddEffect(effect, actor);
                        break;
                }
                break;
            case Team.enemies:
                switch (hexAmount)
                {
                    case 1:
                        effect = new SpeedDebuffEffect(config.speedDebuffFlatAmount, EffectTrigger.none,
                            EffectPriority.First, EffectStack.infinite);
                        manager.AddEffect(effect, actor);
                        break;
                    case 2:
                        effect = new DefenseDebuffEffect(EffectTrigger.OnDamageMitigation, EffectPriority.First, config.defenseBuffFlatAmount,
                            EffectStack.infinite);
                        manager.AddEffect(effect, actor);
                        break;
                    case 3:
                        effect = new DamageDebuffEffect(config.damageBuffFlatAmount,
                            EffectTrigger.OnDamageSend | EffectTrigger.OnTurnEnd, EffectPriority.First,
                            EffectStack.infinite);
                        manager.AddEffect(effect, actor);
                        break;
                }
                break;
            case Team.npc:
                break;
        }

        onComplete?.Invoke();
        yield return null;
    }
    
    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.MoveRange;
    }

    public override bool IsReady()
    {
        if (selectedTargets.Count >= numberOfTargets)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool AddTarget(ITargettable target)
    {
        selectedTargets.Clear();
        selectedTargets.Add(target);
        return true;
    }
}
