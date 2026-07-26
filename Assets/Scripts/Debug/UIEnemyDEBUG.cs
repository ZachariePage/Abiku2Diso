using UnityEngine;

public class UIEnemyDEBUG : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    private int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleLoop.Instance.onCombatStart += SelfDelete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Execute()
    {
        index = index % enemyPrefabs.Length;
        GameObject enemyPrefab = enemyPrefabs[index];
        index++;
        
        SpawnEnemeyAction action = new SpawnEnemeyAction(enemyPrefab);
            
        FindAnyObjectByType<BattleLoop>().SetPendingAction(action);
    }

    public void SelfDelete()
    {
        Destroy(gameObject);
    }
}
