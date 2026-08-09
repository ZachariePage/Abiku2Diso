using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseOmen : AbilityAction, IChoiceGatedAction
{ 
    private FalseOmenTemplateSO config;
    private ElementSO chosenElement;

    public FalseOmen(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range, 
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, FalseOmenTemplateSO config)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
        this.config = config;
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Choice;
    }
    public IReadOnlyList<IActionOption> GetOptions()
    {
        List<IActionOption> options = new List<IActionOption>(config.availableStances.Length);
        foreach (StanceOption pair in config.availableStances)
        {
            options.Add(new ActionOption<ElementSO>(pair.visual.id, pair.visual.displayName, pair.stance, pair.visual.icon));
        }
        return options;
    }

    public bool SelectOption(IActionOption option)
    {
        if (option is not IActionOption<ElementSO> typed) return false;
        chosenElement = typed.GetValue();
        return true;
    }

    public bool HasPendingChoice()
    {
        return true;
    }

    public bool StartActionImmediately()
    {
        return false;
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
        ChangeAttackElementEffect newEffect = new ChangeAttackElementEffect(chosenElement.GetElementType(),
            EffectTrigger.OnDamageSend, EffectPriority.First, EffectStack.oneMax, 1);
        
        selectedTargets[0].GetActor().GetComponent<ActorEffectManager>().AddEffect(newEffect, actor);
        onComplete?.Invoke();
        yield break;
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
