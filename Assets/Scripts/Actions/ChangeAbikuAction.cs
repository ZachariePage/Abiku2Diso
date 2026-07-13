using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAbikuAction : BattleAction, ICostGatedAction
{
    private AbikuTrio owner;

    public ChangeAbikuAction(AbikuTrio owner)
    {
        this.owner = owner;
    }

    public override TargetMode TargetMode()
    {
        return global::TargetMode.Instant;
    }

    public override IEnumerable<ITargettable> GetValidTargets()
    {
        return null;
    }

    public override IEnumerable<GridCell> GetReachableCells()
    {
        return null;
    }

    public override bool TryExecute(ITargettable target)
    {
        return true;
    }
    
    public override IEnumerator Execute(Action onComplete)
    {
        AbikuTrio trio = selectedTargets[0] as AbikuTrio;
        if (trio == null)
        {
            Debug.LogError("ChangeAbikuAction: targetCell is not AbikuTrio");
            onComplete?.Invoke();
            yield return null;
        }
        trio.ChangeAbiku();
        
        onComplete?.Invoke();
        yield return null;
    }

    // public override void Execute()
    // {
    //     AbikuTrio trio = selectedTargets[0] as AbikuTrio;
    //     if (trio == null)
    //     {
    //         Debug.LogError("ChangeAbikuAction: targetCell is not AbikuTrio");
    //         return;
    //     }
    //     trio.ChangeAbiku();
    // }

    public override CellHighlightState GetHighlightState()
    {
        return CellHighlightState.None;
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

    public int ManaCost()
    {
        return 0;
    }

    public object Performer()
    {
        return owner;
    }

    public BattleActionType GetActionType()
    {
        return BattleActionType.ability;
    }
}
