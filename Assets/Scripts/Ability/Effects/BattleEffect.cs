using System;
using System.Collections;
using UnityEngine;

public abstract class BattleEffect
{
    protected bool isFinished;
    public abstract IEnumerator OnTurnStart();
    public abstract bool IsFinished();
    
    //will add later if we want to have area trap
    //public virtual void OnUnitMoved(GridActor actor) { }
    public event Action Finished;

    protected void Complete()
    {
        Finished?.Invoke();
    }

}
