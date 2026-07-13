using System;
using System.Collections;
using UnityEngine;

public abstract class BattleEffect
{
    protected bool isFinished;
    public abstract IEnumerator OnTurnStart();
    public abstract bool IsFinished();
    
    public event Action Finished;

    protected void Complete()
    {
        Finished?.Invoke();
    }

}
