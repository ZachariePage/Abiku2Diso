using UnityEngine;

public enum TargettableTargetType
{
    monobehaviour,
    data
}
public interface ITargettable
{
    public void Select();
    public void Deselect();
    public void Highlight(CellHighlightState mode);
    public void UnHighlight();
    
    void AddHighlight(object source, CellHighlightState state);
    void RemoveHighlight(object source);
    
    TargettableTargetType GetTargetType();
    
    public GridCell GetTargetCell();

    public Vector2 GetWorldPosition();
    
    public GridActor GetActor();
}
