using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityAction : BattleAction, ICostGatedAction
{
    [SerializeField] protected GridActor actor;
    [SerializeField] protected int range;
    [SerializeField] protected TargetingStrategySO direction;
    [SerializeField] protected TargetTypeStrategySO targetAllowed;
    [SerializeField] protected int numberOfTargets;
    [SerializeField] protected ElementSO element;
    ISpellCaster caster;

    public AbilityAction(ISpellCaster caster, GridActor actor, int range, TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
    {
        this.actor = actor;
        this.range = range;
        this.direction = direction;
        this.targetAllowed = targetAllowed;
        this.numberOfTargets = numberOfTargets;
        this.element = element;
        this.caster = caster;
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
        return null;
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

    // THIS SHOULD BE ADDED TO A DAMAGINGABILITY SUBCLASS OF ABILITYACTION ima do it later
    protected List<DamageInfo> DealDamageToTargets(IEnumerable<ITargettable> targets, int damage)
    {
        List<DamageInfo> results = new();
        foreach (var target in targets)
        {
            if (target is IDamageable damageable)
            {
                results.Add(damageable.TakeDamage(actor, this, damage, element.GetElementType()));
            }
            else if (target is GridCell cell)
            {
                GridActor occupant = cell.GetActorOnCell();
                if (occupant == null)
                    continue;

                if (occupant.TryGetComponent<IDamageable>(out var dam))
                {
                    results.Add(dam.TakeDamage(actor, this, damage, element.GetElementType()));
                }
            }
        }
        CheckAndTriggerEncore(results);

        return results;
    }
    
    protected void CheckAndTriggerEncore(IEnumerable<DamageInfo> damageInfos)
    {
        foreach (DamageInfo damageInfo in damageInfos)
        {
            if (damageInfo.Target is AbikuTrio)
                continue;

            if (damageInfo.encoreTriggered)
            {
                TriggerEncore();
                return;
            }
        }
    }
    // end of comment
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
        caster.PutAbilityOnColdown();
    }

    protected void TriggerEncore()
    {
        PlayerBattleStats.Instance.EncoreTriggered();
    }

    public virtual int ManaCost()
    {
        return 0;
    }

    public object Performer()
    {
        return actor;
    }

    public BattleActionType GetActionType()
    {
        return BattleActionType.ability;
    }
    
    public override bool ReadyToUse()
    {
        if (!CanBeUsedNow(BattleLoop.Instance.CurrentPhase) || caster.IsOnColdown())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
