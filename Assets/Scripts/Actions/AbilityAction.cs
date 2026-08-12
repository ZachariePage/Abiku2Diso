using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityAftermathInfo
{
    public ISpellCaster caster;
    public List<ITargettable> targets;
    public AbilityAction UsedAction;
}
[Serializable]
public class AbilityAction : BattleAction, ICostGatedAction
{
    [SerializeField] protected GridActor actor;
    [SerializeField] protected int range;
    [SerializeField] protected TargetingStrategySO direction;
    [SerializeField] protected TargetTypeStrategySO targetAllowed;
    [SerializeField] protected int numberOfTargets;
    [SerializeField] protected ElementSO element;
    [SerializeField] protected int momentumCost;
    protected AbilityTemplateSO template;
    protected ISpellCaster caster;

    public AbilityAction(ISpellCaster caster, GridActor actor,AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
    {
        this.actor = actor;
        this.range = range;
        this.direction = direction;
        this.targetAllowed = targetAllowed;
        this.numberOfTargets = numberOfTargets;
        this.element = element;
        this.caster = caster;
        this.template = template;
        this.momentumCost = template.manaCost;
    }

    public override BattlePhase AllowedPhase()
    {
        return BattlePhase.Combat;
    }

    public override GridActor GetActorOwner()
    {
        return actor;
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Multiple;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        List<GridActor> reachable = GridPathfinder.FindAllActorsWithinRange(actor.GetHoldingCell(), range, direction.allowedDirections);
        
        return reachable;
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
        selectedTargets.Add(target);
        return true;
    }

    public override bool IsOnColdown()
    {
        return caster.IsOnColdown();
    }

    public override void PutOnColdown()
    {
        caster.PutOnColdown();
    }

    public ISpellCaster GetCaster()
    {
        return caster;
    }

    public override int ManaCost()
    {
        return momentumCost;
    }
    public ISpellCaster Caster()
    {
        return caster;
    }

    public override BattleActionType GetActionType()
    {
        return BattleActionType.ability;
    }
    
    public override bool ReadyToUse()
    {
        if (caster is AbikuStance stance && stance.Cheat_GetInfiniteAbility()) return true;
        if (caster.IsOnColdown()) return false;
        UsedActionTracker tracker = caster.GetCooldownTracker();
        bool phaseOk = CanBeUsedNow(BattleLoop.Instance.CurrentPhase);
        bool slotAvailable = !tracker.AbilityUsed();
        bool bonusReady = tracker.HasBonusAction(BattleActionType.ability);
        bool canAfford = ManaCost() <= PlayerBattleStats.Instance.GetMomentum();
        
        // Debug.Log(phaseOk);
        // Debug.Log(slotAvailable);
        // Debug.Log(bonusReady);
        
        return phaseOk && (slotAvailable || bonusReady) && canAfford;
    }

    public override HoverableUIData GetHoverData()
    {
        return template.hoverData;
    }

    public virtual CastingSpellColdownType GetCastingSpellColdownType()
    {
        return CastingSpellColdownType.enemy;
    }
}
