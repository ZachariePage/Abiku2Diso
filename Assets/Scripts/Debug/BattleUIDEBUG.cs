using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIDEBUG : MonoBehaviour
{
    public AbikuTrio[] abikus;
    public GameObject buttonPrefab;
    public GameObject buttonEnemyPrefab;

    public Transform debugTextPosition;
    public GameCue DEBUGCASTINGTEXTCUE;

    private List<GameObject> DEBUGFEEDCUES = new List<GameObject>();
    
    [SerializeField] private int maxDebugCues = 8;
    private GameObject[] debugFeedSlots;
    
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
        objEnemy.transform.position = transform.GetChild(counter).position;

        //feed
        BattleStats.Instance.Subscribe<TurnStartEvent>(onTurnStartEvent);
        BattleStats.Instance.Subscribe<ActionTakenEvent>(onActionTaken);
        
        debugFeedSlots = new GameObject[maxDebugCues];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnText(string text)
    {
        int slotIndex = System.Array.IndexOf(debugFeedSlots, null);
        if (slotIndex == -1)
        {
            return;
        }

        GameObject obj = DEBUGCASTINGTEXTCUE?.Execute(debugTextPosition.position);
        debugFeedSlots[slotIndex] = obj;

        TextMeshProUGUI textMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;

        obj.transform.SetParent(debugTextPosition);

        float newY = debugTextPosition.position.y - (slotIndex * 50);
        obj.transform.position = new Vector3(obj.transform.position.x, newY, obj.transform.position.z);

        StartCoroutine(RemoveCueFromList(obj, slotIndex));
    }

    public void onTurnStartEvent(TurnStartEvent evento)
    {
        SpawnText($"Actor {evento.Actor.ToString()} turn starting");
    }

    public void onActionTaken(ActionTakenEvent evento)
    {
        string targetList = "";
        foreach (var target in evento.Targets)
        {
            targetList += target + ", ";
        }
        
        string newText = $"Actor {evento.Actor} took action {evento.Action} on target {targetList}";
        SpawnText(newText);
    }

    public IEnumerator RemoveCueFromList(GameObject obj, int slotIndex)
    {
        yield return new WaitForSeconds(1f);
        if (debugFeedSlots[slotIndex] == obj)
        {
            debugFeedSlots[slotIndex] = null;
        }
    }
}
