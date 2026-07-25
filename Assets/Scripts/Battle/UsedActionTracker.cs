using System;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionType
{
    move,
    changeStance,
    ability
}
public interface ICostGatedAction
{
    int ManaCost();
    object Performer();
    
    BattleActionType GetActionType();
}
public class UsedActionTracker
{ 
    private List<object> _stanceWhoPlayed = new List<object>();
    private readonly HashSet<(object performer, BattleActionType type)> used = new();
    private GridActor selectedAbiku = null;
    public bool HasUsed(object performer, BattleActionType type)
    {
        object actualPerformer = performer;
        if (actualPerformer is AbikuTrio abiku)
        {
            actualPerformer = abiku.StanceStateMachine.CurrentState;
        }
        Debug.Log(actualPerformer);
        return used.Contains((actualPerformer, type));
    }

    public void MarkUsed(object performer, BattleActionType type)
    {
        object actualPerformer = performer;
        if (actualPerformer is AbikuTrio abiku)
        {
            actualPerformer = abiku.StanceStateMachine.CurrentState;
        }
        Debug.Log(actualPerformer);
        used.Add((actualPerformer, type));
    }

    public void ClearUsed(object performer, BattleActionType type)
    {
        used.Remove((performer, type));
    }

    public bool StanceHasUsed(object performer)
    {
        object actualPerformer = performer;
        if (actualPerformer is AbikuTrio abiku)
        {
            actualPerformer = abiku.StanceStateMachine.CurrentState;
        }
        Debug.Log(actualPerformer);
        return _stanceWhoPlayed.Contains(actualPerformer);
    }

    public void StanceMarkUsed(object performer)
    {
        object actualPerformer = performer;
        if (actualPerformer is AbikuTrio abiku)
        {
            actualPerformer = abiku.StanceStateMachine.CurrentState;
        }
        Debug.Log(actualPerformer);
        _stanceWhoPlayed.Add(actualPerformer);
    }

    public void ClearUsedStance(object performer)
    {
        object actualPerformer = performer;
        if (actualPerformer is AbikuTrio abiku)
        {
            actualPerformer = abiku.StanceStateMachine.CurrentState;
        }
        _stanceWhoPlayed.Remove(actualPerformer);
        ClearUsed(performer,  BattleActionType.move);
        ClearUsed(performer,  BattleActionType.changeStance);
    }

    public GridActor GetSelectedActor()
    {
        return selectedAbiku;
    }

    public void SetSelectedActor(GridActor actor)
    {
        selectedAbiku = actor;
    }

    public void ClearSelectedActor()
    {
        selectedAbiku = null;
    }
    public void ResetTurn()
    {
        used.Clear();
        _stanceWhoPlayed.Clear();
        selectedAbiku = null;
    }
}
