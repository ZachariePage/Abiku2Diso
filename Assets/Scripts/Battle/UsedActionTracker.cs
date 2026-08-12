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


    public bool AbilityUsed()
    {
       return _usedActions.Contains(BattleActionType.ability);
    }

    public bool HasBonusAction(BattleActionType type)
    {
        return _bonusActions.TryGetValue(type, out int number) && number > 0;
    }

    public bool HasAnyBonusAction()
    {
        return _bonusActions.Values.Any(number => number > 0);
    } 

    public void GrantBonusAction(BattleActionType type)
    {
        _bonusActions.TryGetValue(type, out int number);
        _bonusActions[type] = number + 1;
    }

    public void ConsumeBonusAction(BattleActionType type)
    {
        if (_bonusActions.TryGetValue(type, out int c) && c > 0)
        {
            _bonusActions[type] = c - 1;
        }
    }

    public void SetEncoreTriggered(bool value)
    {
        _encoreTriggered = value;
    }
    public bool RegisterActionAndCheckTurnOver(BattleActionType type)
    {
        bool isMoveOrStance = IsMoveOrStance(type);
        bool alreadyUsedSlot = false;
        
        if (isMoveOrStance)
        {
            alreadyUsedSlot = MoveOrStanceUsed();
        }
        else
        {
            alreadyUsedSlot = AbilityUsed();
        }
        
        bool wasBonus = alreadyUsedSlot && HasBonusAction(type);

        _usedActions.Add(type);

        if (wasBonus)
        {
            ConsumeBonusAction(type);
        }

        if (_encoreTriggered)
        {
            _encoreTriggered = false;
            _usedActions.Remove(BattleActionType.changeStance);
            _usedActions.Remove(BattleActionType.move);
            return false;
        }

        if (isMoveOrStance && !wasBonus) return false;
        if (HasAnyBonusAction()) return false;
        if (cheat_infinite) return false;
        return true; 
    }

    public void ResetTurn()
    {
        _usedActions.Clear();
        _bonusActions.Clear();
        _encoreTriggered = false;
    }
    
    public void MarkMoveOrStanceUsedExternally()
    {
        _usedActions.Add(BattleActionType.move);
        _usedActions.Add(BattleActionType.changeStance);
    }
}
