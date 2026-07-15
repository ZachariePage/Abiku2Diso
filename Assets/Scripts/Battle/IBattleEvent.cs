using UnityEngine;

public interface IBattleEvent { }


public struct DamageEvent : IBattleEvent
{
    public DamageInfo damageInfo;
}


public struct UnitKilledEvent : IBattleEvent
{
    public GridActor Killer;
    public GridActor Victim;
}


public struct ActionTakenEvent : IBattleEvent
{
    public GridActor Actor;
    public ITargettable Target;
    public BattleAction Action;
}

public struct MoveEvent : IBattleEvent
{
    public GridActor Actor;
    public GridCell from;
    public GridCell to;
}
