using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAbility : HealingAbilityAbstract, IHealingSource
{
    private List<GameCue> onAbilityThrownCues = new List<GameCue>();
    private DelayedActionEffect delayedEffect;
    private int delayingTurnTime;
    public HealingAbility(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets,
        ElementSO element, int turnDelay, GameCue[] AbilityThrownCues)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element, turnDelay, AbilityThrownCues)
    {
        foreach (var cue in AbilityThrownCues)
        {
            onAbilityThrownCues.Add(cue);
        }
        delayingTurnTime = turnDelay;
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
        selectedTargets[0].AddHighlight(this, CellHighlightState.Targeted);
        
        if (delayingTurnTime > 0)
        {
            delayedEffect = new DelayedActionEffect(delayingTurnTime);
            delayedEffect.onEventCompletion += ResolveHealing;
            actor.AddBattleEndOfTurnEffect(delayedEffect);
            GetCaster().SetCastingSpell(true, GetCastingSpellColdownType());
        }
        else
        {
            foreach (GameCue cue in onAbilityThrownCues)
            {
                cue?.Execute(actor.GetWorldPosition());
            }
            yield return new WaitForSeconds(2f);
            ResolveHealing();
        }
        
        onComplete?.Invoke();
        yield break;
    }
    
    private void ResolveHealing()
    {
        AbilityAftermathInfo info =  new AbilityAftermathInfo
        {
            caster = GetCaster(),
            targets = selectedTargets,
            UsedAction = this
        };

        HealTargets(selectedTargets, 10);
        
        GetCaster().SetCastingSpell(false, GetCastingSpellColdownType());
        GetCaster().OnAbilityFinished(info);
        
        selectedTargets[0].RemoveHighlight(this);
        delayedEffect = null;
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

    public string DisplayName => template.DisplayName;
    public GridActor SourceActor => GetActorOwner();
}
