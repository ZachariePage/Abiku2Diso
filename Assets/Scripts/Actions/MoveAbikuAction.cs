using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MoveAbikuAction : BattleAction, ICostGatedAction
{
    private AbikuTrio abiku;

    public MoveAbikuAction(AbikuTrio abiku)
    {
        this.abiku = abiku;
    }


    public override TargetMode TargetMode()
    {
        return global::TargetMode.Single;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        TrioDefinition def = abiku.GetTrioDefinition();
        List<GridCell> reachable = GridPathfinder.GetReachableCells(abiku.GetHoldingCell(), def.moveRange, def.moveDirection);
        return reachable;
    }

    public override IEnumerable<GridCell> GetReachableCells()
    {
        TrioDefinition def = abiku.GetTrioDefinition();
        List<GridCell> reachable = GridPathfinder.GetReachableCells(abiku.GetHoldingCell(), def.moveRange, def.moveDirection, true);
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
}
