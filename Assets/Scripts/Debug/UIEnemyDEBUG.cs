using UnityEngine;

public class UIEnemyDEBUG : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Execute()
    {
        SpawnEnemeyAction action = new SpawnEnemeyAction(enemyPrefab);
            
        FindAnyObjectByType<BattleLoop>().SetPendingAction(action);
    }
}
