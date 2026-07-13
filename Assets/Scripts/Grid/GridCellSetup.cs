using UnityEngine;

/// <summary>
/// Placed by the map editor on each cell GameObject.
/// GridCellMono reads this in Start() to configure the GridCell it creates.
/// </summary>
public class GridCellSetup : MonoBehaviour
{
    public CellTerrain terrain    = CellTerrain.grass;
    public bool        isWalkable = true;
}
