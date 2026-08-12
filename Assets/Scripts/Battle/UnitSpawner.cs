using UnityEngine;
using UnityEngine.Serialization;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance { get; private set; }
    public GameObject stanceBattleMenu;
    public GameObject choicePromptMenu;
    
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

    public GridActor SpawnActor(GameObject go, GridCell cell)
    {
        GameObject newUnit = Instantiate(go, cell.WorldPosition, Quaternion.identity);
        GridActor actor = newUnit.GetComponent<GridActor>();
        return actor;
    }
    public AbikuTrio SpawnTrioAbiku(TrioDefinition def, GridCell cell)
    {
        GameObject newUnit = Instantiate(def.prefab, cell.WorldPosition, Quaternion.identity);
        AbikuTrio unit = newUnit.GetComponent<AbikuTrio>();
        
        unit.SetHoldingCell(cell);
        unit.SetTrioDefinition(def);
        
        cell.SetActorOnCell(unit);
        
        unit.Initialize();

        SpawnChoicePromptMenu(unit);
        return unit;
    }

    public void SpawnStanceMenu(AbikuStance stance, AbikuTrio unit)
    {
        GameObject newMenu = Instantiate(stanceBattleMenu, unit.GetWorldPosition(),Quaternion.identity ,unit.gameObject.transform);
        StanceBattleMenu menu = newMenu.GetComponent<StanceBattleMenu>();
        menu.owningTrio = unit;
        menu.stance = stance;
    }

    public void SpawnChoicePromptMenu(AbikuTrio unit)
    {
        GameObject newMenu = Instantiate(choicePromptMenu, unit.GetWorldPosition(),Quaternion.identity ,unit.gameObject.transform);
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
