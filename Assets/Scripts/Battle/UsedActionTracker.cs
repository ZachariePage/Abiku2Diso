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
    private readonly List<BattleActionType> usedType = new List<BattleActionType>();
    private GridActor selectedAbiku = null;
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

    public bool HasUsedType(BattleActionType type)
    {
        return usedType.Contains(type);
    }

    public void MarkUsedType(BattleActionType type)
    {
        usedType.Add(type);
    }

    public void ClearUsedType(BattleActionType type)
    {
        usedType.Remove(type);
    }

    public GridActor GetSelectedActor()
    {
        return selectedAbiku;
    }

    public void SetSelectedActor(GridActor actor)
    {
        selectedAbiku = actor;
    }
    public void ResetTurn()
    {
        used.Clear();
        usedType.Clear();
        selectedAbiku = null;
    }
}
