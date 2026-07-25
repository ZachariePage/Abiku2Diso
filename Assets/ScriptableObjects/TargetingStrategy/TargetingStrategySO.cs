using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    normal,
    ray
}
[CreateAssetMenu(menuName = "Strategy/TagetingStrategy")]
public class TargetingStrategySO : ScriptableObject
{
    public MovementDirections allowedDirections;
    [Header("tooltip basically does it follow the same direction or each cell it check allowed direction. For exemple cardinal normal give a losange while direction ray gives a ray")]
    public DirectionType directionType;

    public IEnumerable<GridCell> GetReachableCells(GridCell origin, int maxMove, MovementDirections direction, DirectionType type, bool ignoreOccupancy)
    {
        List<GridCell> reachable = new List<GridCell>();
        switch (type)
        {
            case DirectionType.normal:
                reachable = GridPathfinder.GetReachableCells(origin, maxMove, direction, ignoreOccupancy);
                return reachable;
            case DirectionType.ray:
                reachable = GridPathfinder.GetReachableCellsRay(origin, maxMove, direction, ignoreOccupancy);
                return reachable;
            default:
                return reachable;
        }
    }
    
    public IEnumerable<GridCell> FindCellsWithinRange(GridCell origin, int maxMove, MovementDirections direction, DirectionType typeDirection,TargetType targetType,  bool ignoreOccupancy)
    {
        List<GridCell> reachable = new List<GridCell>();
        switch (typeDirection)
        {
            case DirectionType.normal:
                reachable = GridPathfinder.FindCellsWithinRange(origin, maxMove, targetType, direction, ignoreOccupancy);
                return reachable;
            case DirectionType.ray:
                reachable = GridPathfinder.FindCellsWithinRangeRay(origin, maxMove, targetType, direction, ignoreOccupancy);
                return reachable;
            default:
                return reachable;
        }
    }
}
