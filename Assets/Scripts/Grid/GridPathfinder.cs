using System;
using System.Collections.Generic;
[Flags]
public enum MovementDirections
{
    None = 0,

    Forward = 1 << 0, 
    Backward = 1 << 1,  
    Right = 1 << 2, 
    Left = 1 << 3,  

    ForwardRight = 1 << 4, 
    ForwardLeft = 1 << 5,  
    BackwardRight = 1 << 6, 
    BackwardLeft = 1 << 7, 

    Cardinals = Forward | Backward | Right | Left,
    Diagonals = ForwardRight | ForwardLeft | BackwardRight | BackwardLeft,
    All = Cardinals | Diagonals,
}
public static class GridPathfinder
{
    public static List<GridCell> GetReachableCells(GridCell origin, int maxMove, MovementDirections allowed = MovementDirections.All, bool ignoreOccupancy = false)
    {
        var reachable = new List<GridCell>();
        var visited = new Dictionary<GridCell, int>();

        var queue = new Queue<(GridCell cell, int movesLeft)>();
        queue.Enqueue((origin, maxMove));
        visited[origin] = 0;

        while (queue.Count > 0)
        {
            var (current, movesLeft) = queue.Dequeue();

            for (int i = 0; i < current.Neighbours.Count; i++)
            {
                GridCell neighbour = current.Neighbours[i];

                if (!IsAllowed(current, neighbour, allowed)) continue;
                if (ignoreOccupancy)
                {
                    if (!neighbour.IsWalkable) continue;
                }
                else
                {
                    if (!IsPassable(neighbour)) continue;
                }

                int newCost  = (maxMove - movesLeft) + CostOf(neighbour);
                int remaining = maxMove - newCost;

                if (visited.TryGetValue(neighbour, out int best) && best <= newCost) continue;

                visited[neighbour] = newCost;

                if (remaining >= 0)
                {
                    reachable.Add(neighbour);
                    if (remaining > 0)
                        queue.Enqueue((neighbour, remaining));
                }
            }
        }

        return reachable;
    }
    
    public static List<GridActor> FindAllActorsWithinRange(GridCell origin, int maxRange, MovementDirections allowed = MovementDirections.All, bool ignoreOccupancy = true)
    {
        var actors  = new List<GridActor>();
        var visited = new Dictionary<GridCell, int>();
 
        var queue = new Queue<(GridCell cell, int movesLeft)>();
        queue.Enqueue((origin, maxRange));
        visited[origin] = 0;
 
        while (queue.Count > 0)
        {
            var (current, movesLeft) = queue.Dequeue();
 
            for (int i = 0; i < current.Neighbours.Count; i++)
            {
                GridCell neighbour = current.Neighbours[i];
 
                if (!IsAllowed(current, neighbour, allowed)) continue;
 
                if (ignoreOccupancy)
                {
                    if (!neighbour.IsWalkable) continue;
                }
                else
                {
                    if (!IsPassable(neighbour)) continue;
                }
 
                int newCost   = (maxRange - movesLeft) + CostOf(neighbour);
                int remaining = maxRange - newCost;
 
                if (visited.TryGetValue(neighbour, out int best) && best <= newCost) continue;
 
                visited[neighbour] = newCost;
 
                if (remaining < 0) continue;
 
                GridActor actor = neighbour.GetActorOnCell();
                if (actor != null)
                {
                    actors.Add(actor);
                }
 
                if (remaining > 0)
                {
                    queue.Enqueue((neighbour, remaining));
                }
            }
        }
 
        return actors;
    }
    
    public static List<GridCell> FindCellsWithinRange(
        GridCell origin,
        int maxRange,
        TargetType targetTypes,
        MovementDirections allowed = MovementDirections.All,
        bool ignoreOccupancy = true)
    {
        var result = new List<GridCell>();
        var visited = new Dictionary<GridCell, int>();

        var queue = new Queue<(GridCell cell, int movesLeft)>();
        queue.Enqueue((origin, maxRange));
        visited[origin] = 0;

        while (queue.Count > 0)
        {
            var (current, movesLeft) = queue.Dequeue();

            for (int i = 0; i < current.Neighbours.Count; i++)
            {
                GridCell neighbour = current.Neighbours[i];

                if (!IsAllowed(current, neighbour, allowed))
                    continue;

                if (ignoreOccupancy)
                {
                    if (!neighbour.IsWalkable)
                        continue;
                }
                else
                {
                    if (!IsPassable(neighbour))
                        continue;
                }

                int newCost = (maxRange - movesLeft) + CostOf(neighbour);
                int remaining = maxRange - newCost;

                if (visited.TryGetValue(neighbour, out int best) && best <= newCost)
                    continue;

                visited[neighbour] = newCost;

                if (remaining < 0)
                    continue;

                if (MatchesTargetType(neighbour, targetTypes))
                {
                    result.Add(neighbour);
                }

                if (remaining > 0)
                {
                    queue.Enqueue((neighbour, remaining));
                }
            }
        }

        return result;
    }
    private static bool MatchesTargetType(GridCell cell, TargetType targetTypes)
    {
        GridActor actor = cell.GetActorOnCell();
        
        if ((targetTypes & TargetType.EmptyCell) != 0 &&
            cell.IsWalkable &&
            actor == null)
        {
            return true;
        }

        if (actor == null)
            return false;

        // if ((targetTypes & TargetType.Terrain) != 0 &&
        //     actor is Terrain)
        // {
        //     return true;
        // }

        if ((targetTypes & TargetType.Allies) != 0 &&
            actor is AbikuTrio)
        {
            return true;
        }

        if ((targetTypes & TargetType.Enemies) != 0 &&
            actor is Enemy)
        {
            return true;
        }

        return false;
    }

    public static List<GridCell> FindPath(GridCell origin, GridCell destination, MovementDirections allowed = MovementDirections.All)
    {
        if (origin == destination) return new List<GridCell>();

        var cameFrom = new Dictionary<GridCell, GridCell>();
        var costSoFar = new Dictionary<GridCell, int>();
        var queue = new Queue<GridCell>();

        queue.Enqueue(origin);
        cameFrom[origin]  = null;
        costSoFar[origin] = 0;

        while (queue.Count > 0)
        {
            GridCell current = queue.Dequeue();
            if (current == destination) break;

            for (int i = 0; i < current.Neighbours.Count; i++)
            {
                GridCell neighbour = current.Neighbours[i];

                if (!IsAllowed(current, neighbour, allowed)) continue;
                if (!IsPassable(neighbour) && neighbour != destination) continue;

                int newCost = costSoFar[current] + CostOf(neighbour);

                if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                {
                    costSoFar[neighbour] = newCost;
                    cameFrom[neighbour]  = current;
                    queue.Enqueue(neighbour);
                }
            }
        }

        if (!cameFrom.ContainsKey(destination)) return new List<GridCell>();

        var path = new List<GridCell>();
        GridCell step = destination;
        while (step != origin) { path.Add(step); step = cameFrom[step]; }
        path.Reverse();
        return path;
    }

    public static bool TryGetPathWithinRange(GridCell origin, GridCell destination, int maxMove, MovementDirections allowed, out List<GridCell> path)
    {
        path = FindPath(origin, destination, allowed);

        if (path.Count == 0 && origin != destination) return false;

        return path.Count <= maxMove;
    }

    // Derives the direction from current to neighbour and checks against the allowed flags
    private static bool IsAllowed(GridCell current, GridCell neighbour, MovementDirections allowed)
    {
        int dx = neighbour.X - current.X;
        int dz = neighbour.Z - current.Z;

        MovementDirections dir = GetDirection(dx, dz);

        return (allowed & dir) != 0;
    }

    private static MovementDirections GetDirection(int dx, int dz)
    {
        if (dx ==  0 && dz ==  1) return MovementDirections.Forward;
        if (dx ==  0 && dz == -1) return MovementDirections.Backward;
        if (dx ==  1 && dz ==  0) return MovementDirections.Right;
        if (dx == -1 && dz ==  0) return MovementDirections.Left;
        if (dx ==  1 && dz ==  1) return MovementDirections.ForwardRight;
        if (dx == -1 && dz ==  1) return MovementDirections.ForwardLeft;
        if (dx ==  1 && dz == -1) return MovementDirections.BackwardRight;
        if (dx == -1 && dz == -1) return MovementDirections.BackwardLeft;

        return MovementDirections.None;
    }

    private static bool IsPassable(GridCell cell)
    {
        return cell.IsWalkable && cell.IsEmpty();
    }

    private static int CostOf(GridCell cell)
    {
        return 1;
    }
}