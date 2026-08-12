using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CellTerrain
{
    none,
    grass,
    water,
    mountain,
}
public class GridCell : ITargettable, IDamageable
{
    public int X { get; private set; }
    public int Z { get; private set; }
    public Vector2 WorldPosition { get; private set; }

    public bool IsWalkable { get; set; } = true;
    public CellTerrain Terrain { get; set; } = CellTerrain.grass;
    
    public CellHighlightState HighlightState { get; set; } = CellHighlightState.None;
    
    private Dictionary<object, List<CellHighlightState>> highlights = new();

    public IReadOnlyCollection<CellHighlightState> ActiveHighlights =>
        highlights.Values.SelectMany(x => x).ToList();
    
    public List<GridCell> Neighbours { get; private set; } = new List<GridCell>(8);

    private GridActor actorOnCell;

    public GridActor GetActorOnCell()
    {
        return actorOnCell;
    }

    public void SetActorOnCell(GridActor actor)
    {
        actorOnCell = actor;
    }

    public void AddNeighbour(GridCell neighbour)
    {
        if (!Neighbours.Contains(neighbour))
        {
            Neighbours.Add(neighbour);
        }
    }

    public GridCell(int x, int z, Vector2 worldPosition)
    {
        X = x;
        Z = z;
        WorldPosition = worldPosition;
    }

    public bool IsEmpty()
    {
        return actorOnCell == null;
    }

    public void EmptyCell()
    {
        actorOnCell = null;
    }

    public void Select()
    {
        Highlight(CellHighlightState.Selected);
        HoverableUIData data = new HoverableUIData
        (
            "Cell",
            $"Terrain type = {Terrain}"
            
        );
        HoverTooltip.Instance.CreateTooltip(this, data, TooltipType.GridCellInformation, GetWorldPosition());
    }

    public void Deselect()
    {
        UnHighlight();
        HoverTooltip.Instance.RemoveTooltip(this);
    }

    public void Highlight(CellHighlightState mode)
    {
        TacticalGrid.Instance.HighlightCell(this, mode);
    }

    public void UnHighlight()
    {
        TacticalGrid.Instance.UnHighlightCell(this);
    }

    public void AddHighlight(object source, CellHighlightState state)
    {
        if (!highlights.TryGetValue(source, out var states))
        {
            states = new List<CellHighlightState>();
            highlights[source] = states;
        }

        states.Add(state);
        Refresh();
    }

    public void RemoveHighlight(object source)
    {
        if (highlights.Remove(source))
        {
            Refresh();
        }
    }
    
    private void Refresh()
    {
        TacticalGrid.Instance.RefreshCellHighlight(this);
    }
    
    private static int GetPriority(CellHighlightState state)
    {
        switch (state)
        {
            case CellHighlightState.None:
                return 0;
            case CellHighlightState.Hovered:
                return 1;
            case CellHighlightState.MoveRange:
                return 2;
            case CellHighlightState.AttackRange:
                return 3;
            case CellHighlightState.Selected:
                return 4;
            case CellHighlightState.Targeted:
                return 5;
            default:
                return -1;
        }
    }
    
    public virtual TargettableTargetType GetTargetType()
    {
        return TargettableTargetType.data;
    }

    public GridCell GetTargetCell()
    {
        return this;
    }

    public Vector2 GetWorldPosition()
    {
        return WorldPosition;
    }

    public GridActor GetActor()
    {
        return GetActorOnCell();
    }

    public override string ToString()
    {
        return $"Cell({X}, {Z}) @ {WorldPosition}";
    }

    public DamageInfo TakeDamage(GridActor source, IDamageSource damageSource, float damage, Element element)
    {
        DamageInfo result = new DamageInfo();
        if (actorOnCell != null)
        {
            if (actorOnCell is IDamageable damageable)
            {
                result = damageable.TakeDamage(source, damageSource, damage, element);
            }
        }
        return result;
    }

    public HealingInfo Heal(GridActor source, IHealingSource healingSource, float heal, Element element)
    {
        HealingInfo result = new HealingInfo();
        if (actorOnCell != null)
        {
            if (actorOnCell is IDamageable damageable)
            {
                result = damageable.Heal(source, healingSource, heal, element);
            }
        }
        return result;
    }
    
    
    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        Debug.LogError("I have no idea why this would ever be called, unliked the takedamage above, prob should make an interface IDealDamage");
        if (actorOnCell != null)
        {
            if (actorOnCell is IDamageable damageable)
            {
                return damageable.ModifyOutgoingDamage(ctx);
            }
        }

        return ctx;
    }

    public bool IsWounded()
    {
        if (actorOnCell != null)
        {
            if (actorOnCell is IDamageable damageable)
            {
                return damageable.GetHealth().IsWounded();
            }
        }
        return false;
    }

    public Health GetHealth()
    {
        if (actorOnCell != null)
        {
            if (actorOnCell is IDamageable damageable)
            {
                return damageable.GetHealth();
            }
        }
        
        return null;
    }
}