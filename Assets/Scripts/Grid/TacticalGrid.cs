using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TacticalGrid : MonoBehaviour
{
    [Header("Cell Size")]
    [SerializeField] public float cellSize = 1f;

    [Header("tilmeap")]
    [SerializeField] private Tilemap tilemap;
    
    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private CellHighlighter _highlighter;

    private Dictionary<Vector2Int, GridCell> _grid = new Dictionary<Vector2Int, GridCell>();
    
    public event Action onGridComplete;

    public static TacticalGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        tilemap = FindAnyObjectByType<Tilemap>();
        _highlighter = GetComponentInChildren<CellHighlighter>();
    }

    private void Start()
    {
        StartCoroutine(BuildNeighbourLinksNextFrame());
    }

    private IEnumerator BuildNeighbourLinksNextFrame()
    {
        yield return null;
        BuildGrid();
        BuildNeighbourLinks();
        onGridComplete?.Invoke();
        Debug.Log($"[TacticalGrid] Grid ready — {_grid.Count} cells registered.");
    }

    private void BuildGrid()
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
                continue;

            GridCell cell = new GridCell(
                pos.x,
                pos.y,
                tilemap.GetCellCenterWorld(pos)
            );

            RegisterCell(cell);
        }
    }

    private void BuildNeighbourLinks()
    {
        int[] dx = {  1, -1,  0,  0,  1, -1,  1, -1 };
        int[] dz = {  0,  0,  1, -1,  1,  1, -1, -1 };

        foreach (GridCell cell in _grid.Values)
        {
            for (int i = 0; i < dx.Length; i++)
            {
                Vector2Int neighbourKey = new Vector2Int(cell.X + dx[i], cell.Z + dz[i]);

                if (_grid.TryGetValue(neighbourKey, out GridCell neighbour))
                {
                    cell.AddNeighbour(neighbour);
                }
            }
        }
    }

    public void RegisterCell(GridCell cell)
    {
        Vector2Int key = new Vector2Int(cell.X, cell.Z);

        if (_grid.ContainsKey(key))
        {
            Debug.LogWarning($"[TacticalGrid] Duplicate cell at ({cell.X},{cell.Z}) — overwriting.");
        }

        _grid[key] = cell;
    }

    public GridCell GetCell(int x, int z)
    {
        Vector2Int key = new Vector2Int(x, z);

        if (!_grid.TryGetValue(key, out GridCell cell))
        {
            Debug.LogWarning($"[TacticalGrid] No cell at ({x},{z}).");
            return null;
        }

        return cell;
    }

    public GridCell GetCellFromWorldPosition(Vector2 worldPosition)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPosition);

        return GetCell(cellPos.x, cellPos.y);
    }

    public List<GridCell> GetAllCells()
    {
        return new List<GridCell>(_grid.Values);
    }

    public List<GridCell> GetAllEmptyCells()
    {
        var list = new List<GridCell>();

        foreach (GridCell cell in _grid.Values)
        {
            if (cell.IsEmpty())
            {
                list.Add(cell);
            }
        }

        return list;
    }

    public List<GridCell> GetNeighbours(GridCell cell)
    {
        return cell.Neighbours;
    }

    public bool IsInBounds(int x, int z)
    {
        return _grid.ContainsKey(new Vector2Int(x, z));
    }

    public void SelectCell(GridCell cell)
    {
        _highlighter.SetHighlight(cell, CellHighlightState.Selected);
    }

    public void HighlightCell(GridCell cell, CellHighlightState mode)
    {
        _highlighter.SetHighlight(cell, mode);
    }

    public void UnHighlightCell(GridCell cell)
    {
        _highlighter.SetHighlight(cell, CellHighlightState.None);
    }

    public void RefreshCellHighlight(GridCell cell)
    {
        _highlighter.RefreshView(cell);
    }

    public void DeselectAllCells()
    {
        _highlighter.ClearAll();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos || _grid == null) return;

        Gizmos.color = new Color(0f, 1f, 0.8f, 0.35f);

        foreach (GridCell cell in _grid.Values)
        {
            Vector3 center = new Vector3(cell.WorldPosition.x, cell.WorldPosition.y, 0f);
            Gizmos.DrawWireCube(center, new Vector3(cellSize * 0.95f, cellSize * 0.95f, 0f));
        }
    }
}