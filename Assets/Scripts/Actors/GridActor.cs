using System;
using UnityEngine;

public class GridActor : MonoBehaviour, ITargettable
{
    private GridCell holdingCell;
    
    public event Action onSelection;
    public event Action onDeselection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Initialize()
    {
        
    }
    //getter setter
    public GridCell GetHoldingCell()
    {
        return holdingCell;
    }

    public void SetHoldingCell(GridCell cell)
    {
        holdingCell = cell;
        cell.SetActorOnCell(this);
    }

    public virtual void Select()
    {
        onSelection?.Invoke();
    }

    public virtual void Deselect()
    {
        onDeselection?.Invoke();
    }

    public virtual void Highlight(CellHighlightState mode)
    {
        
    }

    public virtual void UnHighlight()
    {
        
    }

    public void AddHighlight(object source, CellHighlightState state)
    {
        
    }

    public void RemoveHighlight(object source)
    {
        
    }

    public virtual TargettableTargetType GetTargetType()
    {
        return TargettableTargetType.monobehaviour;
    }

    public Vector2 GetWorldPosition()
    {
        return transform.position;
    }
}
