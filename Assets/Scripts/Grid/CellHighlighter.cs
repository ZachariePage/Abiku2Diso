using System;
using System.Collections.Generic;
using UnityEngine;

public enum CellHighlightState
{
    None,
    Hovered,
    Selected,
    MoveRange,
    AttackRange,
    Targeted,
    Reachable,
}

public class CellHighlighter : MonoBehaviour
{
    private TacticalGrid _grid;

    [SerializeField]
    private GameObject highlightPrefab;

    private readonly Dictionary<GridCell, CellHighlightView> _views = new();

    private readonly HashSet<GridCell> _highlightedCells = new();

    private void Awake()
    {
        
    }

    private void Start()
    {
        TacticalGrid.Instance.onGridComplete += SpawnHighlighters;
    }

    private void SpawnHighlighters()
    {
        _grid = GetComponent<TacticalGrid>();

        foreach (GridCell cell in _grid.GetAllCells())
        {
            GameObject obj = Instantiate(
                highlightPrefab,
                cell.WorldPosition + Vector2.up * 0.01f,
                Quaternion.identity,
                transform
            );

            CellHighlightView view = obj.GetComponent<CellHighlightView>();
            view.Init();
            //view.SetState(CellHighlightState.None);
            _views.Add(cell, view);
        }
    }

    public void RefreshView(GridCell cell)
    {
        if (!_views.TryGetValue(cell, out CellHighlightView view))
        {
            Debug.LogWarning("somehow no highlighter view for this cell");
            return;
        }

        view.SetState(cell);
    }
    
    public void SetHighlight(GridCell cell, CellHighlightState state)
    {
        if (state == CellHighlightState.None)
        {
            cell.RemoveHighlight(this);
            _highlightedCells.Remove(cell);
        }
        else
        {
            cell.AddHighlight(this, state);
            _highlightedCells.Add(cell);
        }
    }

    public void SetHighlights(IEnumerable<GridCell> cells, CellHighlightState state)
    {
        foreach (GridCell cell in cells)
        {
            SetHighlight(cell, state);
        }
    }

    public void ClearAll()
    {
        foreach (GridCell cell in _highlightedCells)
        {
            cell.RemoveHighlight(this);
        }
        _highlightedCells.Clear();
    }

    public void Clear(IEnumerable<GridCell> cells)
    {
        foreach (GridCell cell in cells)
        {
            SetHighlight(cell, CellHighlightState.None);
        }
    }
}