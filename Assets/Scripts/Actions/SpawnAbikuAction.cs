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


    public override BattlePhase AllowedPhase()
    {
        return BattlePhase.Preparation;
    }

    public override GridActor GetActorOwner()
    {
        return null;
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
        AbikuTrio unit = UnitSpawner.Instance.SpawnTrioAbiku(_prefabToSpawn.GetTrioDefinition(), targetGridCell);
        BattleLoop.Instance.AddAbikuTrio(unit);
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

    public override bool IsOnColdown()
    {
        return false;
    }

    public override void PutOnColdown()
    {
        
    }

    public override bool ReadyToUse()
    {
        return CanBeUsedNow(BattleLoop.Instance.CurrentPhase);
    }

    public override HoverableUIData GetHoverData()
    {
        return new  HoverableUIData(
            "spawn abiku",
            "meowing"
        );
    }

    public override BattleActionType GetActionType()
    {
        return BattleActionType.debug;
    }
}
