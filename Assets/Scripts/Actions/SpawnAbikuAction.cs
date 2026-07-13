using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnAbikuAction : BattleAction
{
    private AbikuTrio _prefabToSpawn;

    public SpawnAbikuAction(AbikuTrio prefabToSpawn)
    {
        _prefabToSpawn = prefabToSpawn;
    }


    public override TargetMode TargetMode()
    {
        return global::TargetMode.Single;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        return TacticalGrid.Instance.GetAllEmptyCells();
    }

    public override IEnumerable<GridCell> GetReachableCells()
    {
        return TacticalGrid.Instance.GetAllEmptyCells();
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
        GridCell targetGridCell = selectedTargets[0] as GridCell;
        UnitSpawner.Instance.SpawnTrioAbiku(_prefabToSpawn.GetTrioDefinition(), targetGridCell);
        onComplete?.Invoke();
        yield return null;
    }

    // public override void Execute()
    // {
    //     GridCell targetGridCell = selectedTargets[0] as GridCell;
    //     UnitSpawner.Instance.SpawnTrioAbiku(_prefabToSpawn.GetTrioDefinition(), targetGridCell);
    // }

    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.Selected;
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
        selectedTargets.Add(target);
        return true;
    }
}
