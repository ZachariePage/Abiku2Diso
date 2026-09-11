using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum BattleActionType
{
    move,
    changeStance,
    ability,
    debug
}
public interface ICostGatedAction
{
    int ManaCost();
    ISpellCaster Caster();
    BattleActionType GetActionType();
}


public class UsedActionTracker
{
    public bool cheat_infinite = false;
    private HashSet<BattleActionType> _usedActions = new();
    private Dictionary<BattleActionType, int> _bonusActions = new();
    private bool _encoreTriggered;

    public static bool IsMoveOrStance(BattleActionType type)
    {
        return type == BattleActionType.move || type == BattleActionType.changeStance;
    }

    public bool MoveOrStanceUsed()
    {
        return _usedActions.Contains(BattleActionType.move) || _usedActions.Contains(BattleActionType.changeStance);
    }

    public bool CanUseMoveOrStance()
    {
        if(PlayerBattleStats.Instance.HasRemainingEncore()) return true;
        return !(_usedActions.Contains(BattleActionType.move) || _usedActions.Contains(BattleActionType.changeStance));
    }


    public bool AbilityUsed()
    {
       return _usedActions.Contains(BattleActionType.ability);
    }

    public bool CanUseAbility()
    {
        if(PlayerBattleStats.Instance.HasRemainingEncore()) return true;
        return !_usedActions.Contains(BattleActionType.ability);
    }
    

    public void SetEncoreTriggered(bool value)
    {
        _encoreTriggered = value;
    }

    public bool HasMoveLeft()
    {
        if(PlayerBattleStats.Instance.HasRemainingEncore()) return true;
        bool abilityUsed = AbilityUsed();
        bool moveOrStance = MoveOrStanceUsed();
        
        return !abilityUsed || (!moveOrStance);
    }
    public bool RegisterActionAndCheckTurnOver(BattleActionType type)
    {
        bool encoreWasUsed = false;
        switch (type)
        {
            case BattleActionType.move:
            case BattleActionType.changeStance:
                encoreWasUsed = MoveOrStanceUsed();
                break;
            case BattleActionType.ability:
                encoreWasUsed = AbilityUsed();
                break;
        }

        if (encoreWasUsed)
        {
            PlayerBattleStats.Instance.ConsumeEncoreCharge();
        }
        
        _usedActions.Add(type);
        
        if (cheat_infinite) return false;
        return HasMoveLeft(); 
    }

    public void ResetTurn()
    {
        _usedActions.Clear();
        _bonusActions.Clear();
        _encoreTriggered = false;
    }
}
