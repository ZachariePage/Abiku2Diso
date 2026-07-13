using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Attach this to a GameObject placed by hand in the scene.
/// Grid coordinates are read directly from the GameObject's world position.
/// Place your tiles at whole-number positions (1,0,0), (2,0,0), (1,1,0) etc.
/// and the coordinates will match exactly.
/// </summary>
public class GridCellMono : MonoBehaviour
{
    public GridCell Cell { get; private set; }

    private void Start()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.y);

        Cell = new GridCell(x, z, transform.position);
        TacticalGrid.Instance.RegisterCell(Cell);

        if ((math.abs(Cell.X + Cell.Z)) % 2 == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        GridCellSetup setup = GetComponent<GridCellSetup>();
        
        if (setup != null)
        {
            Cell.Terrain = setup.terrain;
            Cell.IsWalkable = setup.isWalkable;
        }
    }
}