using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAbikuAction : BattleAction, ICostGatedAction
{
    private AbikuTrio abiku;
    private ISpellCaster caster;

    public MoveAbikuAction(ISpellCaster caster, AbikuTrio abiku)
    {
        this.abiku = abiku;
        this.caster = caster;
    }


    public override BattlePhase AllowedPhase()
    {
        return BattlePhase.Combat;
    }

    public override GridActor GetActorOwner()
    {
        return abiku;
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Single;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        TrioDefinition def = abiku.GetTrioDefinition();
        List<GridCell> reachable;

        switch (def.directionType)
        {
            case DirectionType.normal:
                reachable = GridPathfinder.GetReachableCells(abiku.GetHoldingCell(), def.moveRange, def.moveDirection);
                break;

            case DirectionType.ray:
                reachable = GridPathfinder.GetReachableCellsRay(abiku.GetHoldingCell(), def.moveRange, def.moveDirection);
                break;
            default:
                reachable = new List<GridCell>();
                break;
        }

        return reachable;
    }

    public override IEnumerable<GridCell> GetReachableCells()
    {
        TrioDefinition def = abiku.GetTrioDefinition();
        List<GridCell> reachable;

        switch (def.directionType)
        {
            case DirectionType.normal:
                reachable = GridPathfinder.GetReachableCells(abiku.GetHoldingCell(), def.moveRange, def.moveDirection, true);
                break;

            case DirectionType.ray:
                reachable = GridPathfinder.GetReachableCellsRay(abiku.GetHoldingCell(), def.moveRange, def.moveDirection, true);
                break;
            default:
                reachable = new List<GridCell>();
                break;
        }

        return reachable;
    }

    public override bool TryExecute(ITargettable target)
    {
        if (!GetValidTargets().Contains(target))
        {
            return false;
        }
        return true;
    }

    public override IEnumerator Execute(Action onComplete)
    {
        abiku.MoveToCell(selectedTargets[0] as GridCell);
        onComplete?.Invoke();
        yield return null;
    }

    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.MoveRange;
    }

    public override bool IsReady()
    {
        if (selectedTargets.Count >= 1)
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

    public override bool IsOnColdown()
    {
        return false;
    }

    public override void PutOnColdown()
    {
        caster.PutAbilityOnColdown();
    }

    public int ManaCost()
    {
        return 0;
    }

    public object Performer()
    {
        return abiku;
    }

    public BattleActionType GetActionType()
    {
        return BattleActionType.move;
    }
    
    public override bool ReadyToUse()
    {
        return CanBeUsedNow(BattleLoop.Instance.CurrentPhase);
    }
}
