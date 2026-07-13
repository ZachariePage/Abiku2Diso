using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance { get; private set; }
    public GameObject abikuMenuPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnTrioAbiku(TrioDefinition def, GridCell cell)
    {
        GameObject newUnit = Instantiate(def.prefab, cell.WorldPosition, Quaternion.identity);
        AbikuTrio unit = newUnit.GetComponent<AbikuTrio>();
        
        unit.SetHoldingCell(cell);
        unit.SetTrioDefinition(def);
        
        cell.SetActorOnCell(unit);
        
        unit.Initialize();
        
        GameObject newCanvas = Instantiate(abikuMenuPrefab, newUnit.transform.position, Quaternion.identity,  newUnit.transform);
        newCanvas.GetComponent<AbikuBattleMenu>().owningTrio = unit;
    }
    
    public Enemy SpawnEnemy(GameObject prefab, GridCell cell)
    {
        GameObject newUnit = Instantiate(prefab, cell.WorldPosition, Quaternion.identity);
        Enemy unit = newUnit.GetComponent<Enemy>();
        
        unit.SetHoldingCell(cell);
        
        cell.SetActorOnCell(unit);
        
        unit.Initialize();
        
        return unit;
    }
}
