using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAbikuAction : BattleAction, ICostGatedAction
{
    private AbikuTrio owner;
    private ISpellCaster caster;

    public ChangeAbikuAction(ISpellCaster caster, AbikuTrio owner)
    {
        this.owner = owner;
        this.caster = caster;
    }

    public override BattlePhase AllowedPhase()
    {
        return BattlePhase.Combat;
    }

    public override GridActor GetActorOwner()
    {
        return owner;
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

        // if (caster.IsOnColdown())
        // {
        //     BattleLoop.Instance.EncoreTriggered();
        // }
        trio.ChangeStance();
        
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

    public override bool IsOnColdown()
    {
        return false;
    }

    public override void PutOnColdown()
    {
        caster.PutOnColdown();
    }

    public override int ManaCost()
    {
        return 0;
    }

    public ISpellCaster Caster()
    {
        return caster;
    }

    public override BattleActionType GetActionType()
    {
        return BattleActionType.changeStance;
    }
    
    public override bool ReadyToUse()
    {
        if (caster is AbikuStance stance && stance.Cheat_GetInfiniteAbility()) return true;
        if (caster.IsOnColdown()) return false;
        UsedActionTracker tracker = caster.GetCooldownTracker();
        bool phaseOk = CanBeUsedNow(BattleLoop.Instance.CurrentPhase);
        bool slotAvailable = tracker.CanUseMoveOrStance();

        if (owner.IsOnLastStance())
        {
            return false;
        }
        return !owner.IsStanceLocked() && phaseOk && (slotAvailable);
    }

    public override HoverableUIData GetHoverData()
    {
        return new  HoverableUIData(
            "change abiku",
            "meowing"
        );
    }
}
