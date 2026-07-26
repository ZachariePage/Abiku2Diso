using System;
using System.Collections;
using UnityEngine;

public class DelayedActionEffect : BattleEffect
{
    private int _turnsRemaining;

    public event Action onEventCompletion;

    public DelayedActionEffect(int turnsRemaining)
    {
        _turnsRemaining = turnsRemaining;
    }

    public override IEnumerator OnTurnStart()
    {
        _turnsRemaining--;
        {
            if (_turnsRemaining > 0) yield break;
        }

        onEventCompletion?.Invoke();
        isFinished = true;
        yield return null;
    }

    public override bool IsFinished()
    {
        return isFinished;
    }
}
