using UnityEngine;

public class BattleUIDEBUG : MonoBehaviour
{
    public AbikuTrio[] abikus;
    public GameObject buttonPrefab;
    public GameObject buttonEnemyPrefab;
    
    public GameObject enemyPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int counter = 0;
        foreach (AbikuTrio abiku in abikus)
        {
            Vector3 position = new Vector3(transform.position.x + (50 * counter), transform.position.y, transform.position.z);
            
            GameObject obj = Instantiate(buttonPrefab, position, Quaternion.identity, transform);
            UIAbikuDEBUG abikuButton =  obj.GetComponent<UIAbikuDEBUG>();
            obj.transform.position = transform.GetChild(counter).position;

            abikuButton.abikuTrioPrefab = abiku;
            counter++;
        }
        
        Vector3 positionEnemy = new Vector3(transform.position.x + (50 * counter), transform.position.y, transform.position.z);
            
        GameObject objEnemy = Instantiate(buttonEnemyPrefab, positionEnemy, Quaternion.identity, transform);
        UIEnemyDEBUG abikuButtonEnemy = objEnemy.GetComponent<UIEnemyDEBUG>();
        objEnemy.transform.position = transform.GetChild(counter).position;

        abikuButtonEnemy.enemyPrefab = enemyPrefab;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
