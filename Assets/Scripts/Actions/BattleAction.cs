using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IReadOnlyBattleAction
{
    BattlePhase AllowedPhase();
    GridActor GetActorOwner();
    bool CanBeUsedNow(BattlePhase current);
    TargetMode TargetMode();
    IEnumerable<ITargettable> GetValidTargets();
    IEnumerable<GridCell> GetReachableCells();
    CellHighlightState GetHighlightState();
    bool IsReady();
    IReadOnlyList<ITargettable> GetTargets();
    bool IsOnColdown();
    bool ReadyToUse();
    HoverableUIData GetHoverData();
}

public enum TargetMode
{
    Instant,
    Single,
    Multiple
}

[Flags]
public enum TargetType
{
    None = 0,

    EmptyCell = 1 << 0,
    Terrain = 1 << 1,
    Allies = 1 << 2,
    Enemies = 1 << 3,

    Units = Allies | Enemies,
    All = EmptyCell | Terrain | Allies | Enemies
}
[Serializable]
public abstract class BattleAction : IReadOnlyBattleAction
{
    protected bool coldown;
    protected List<ITargettable> selectedTargets = new();

    public abstract BattlePhase AllowedPhase();

    public abstract GridActor GetActorOwner();
    
    public bool CanBeUsedNow(BattlePhase current) => (AllowedPhase() & current) != 0;
    public abstract TargetMode TargetMode();
    public abstract IEnumerable<ITargettable> GetValidTargets();
    
    public abstract IEnumerable<GridCell> GetReachableCells();
    public abstract bool TryExecute(ITargettable target);
    public abstract IEnumerator Execute(System.Action onComplete);

    public abstract CellHighlightState GetHighlightState();
    
    public abstract bool IsReady();

    public IReadOnlyList<ITargettable> GetTargets()
    {
        return selectedTargets;
    }
    
    public abstract bool AddTarget(ITargettable target);

    public abstract bool IsOnColdown();
    public abstract void PutOnColdown();

    public abstract bool ReadyToUse();
    
    public abstract HoverableUIData GetHoverData();

}
