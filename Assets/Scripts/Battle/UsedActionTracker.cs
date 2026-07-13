using System;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionType
{
    move,
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
    private readonly HashSet<(object performer, BattleActionType type)> used = new();
    
    public bool HasUsed(object performer, BattleActionType type)
    {
        return used.Contains((performer, type));
    }

    public void MarkUsed(object performer, BattleActionType type)
    {
        used.Add((performer, type));
    }

    public void ClearUsed(object performer, BattleActionType type)
    {
        used.Remove((performer, type));
    }

    public void ResetTurn()
    {
        used.Clear();
    }
}
