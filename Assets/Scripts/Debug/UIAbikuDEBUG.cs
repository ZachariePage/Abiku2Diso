using UnityEngine;

public class UIAbikuDEBUG : MonoBehaviour
{
    public AbikuTrio abikuTrioPrefab;
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
        SpawnAbikuAction action = new SpawnAbikuAction(abikuTrioPrefab.GetTrioDefinition().prefab.GetComponent<AbikuTrio>());
        
        FindAnyObjectByType<BattleLoop>().SetPendingAction(action);
    }
}
