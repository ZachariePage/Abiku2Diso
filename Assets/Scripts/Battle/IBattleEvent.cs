using System.Collections.Generic;
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
    public List<ITargettable> Targets;
    public BattleAction Action;
}

public struct CheatUsedEvent : IBattleEvent
{
    public string CheatString;
}


public struct MoveEvent : IBattleEvent
{
    public GridActor Actor;
    public GridCell from;
    public GridCell to;
}

public struct GridActorTurnStartEvent : IBattleEvent
{
    public GridActor Actor;
}

public enum Team
{
    allies,
    enemies,
    npc
}
public struct TurnPassEvent : IBattleEvent
{
    public Team team;
}

public struct TurnStartEvent : IBattleEvent
{
    public Team team;
}
