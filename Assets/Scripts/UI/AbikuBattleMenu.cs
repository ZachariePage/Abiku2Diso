using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

public class AbikuBattleMenu : MonoBehaviour
{
    public GameObject Panel;
    
    public GameObject buttonPrefab;
    private List<AbilityButton> buttons = new List<AbilityButton>();
    
    //horrible disgusting afront to god code that hopefully will be changed later
    private GameObject moveButton;
    private GameObject changeAbikuButton;

    public AbikuTrio owningTrio;
    
    void Start()
    {
        owningTrio.onSelection += OpenUI;
        owningTrio.onDeselection += CloseUI;
        owningTrio.onAbilityModify += RefreshButtons;
        Panel.SetActive(false);
        
        CreateButtons();
    }

    private void CreateButtons()
    {
        // buttons.Clear();
        // int counter = 0;
        //
        // //move action
        // Vector3 position2 = Panel.transform.GetChild(counter).position;
        // GameObject obj2 = Instantiate(buttonPrefab, position2, Quaternion.identity, Panel.transform);
        // AbilityButton abikuButton2 =  obj2.GetComponent<AbilityButton>();
        // obj2.transform.position = Panel.transform.GetChild(counter).position;
        // abikuButton2.owningTrio = owningTrio;
        // abikuButton2.action = owningTrio.GetMoveAction();
        // counter++;
        // moveButton = abikuButton2.gameObject;
        //
        // //change stance
        // Vector3 position1 = Panel.transform.GetChild(counter).position;
        // GameObject obj1 = Instantiate(buttonPrefab, position1, Quaternion.identity, Panel.transform);
        // AbilityButton abikuButton1 =  obj1.GetComponent<AbilityButton>();
        // obj1.transform.position = Panel.transform.GetChild(counter).position;
        // abikuButton1.owningTrio = owningTrio;
        // abikuButton1.action = owningTrio.GetChangeAction();
        // counter++;
        // changeAbikuButton = abikuButton1.gameObject;
        //
        // foreach (AbilityAction abilityAction in owningTrio.GetAbilityActions())
        // {
        //     Vector3 position = Panel.transform.GetChild(counter).position;
        //     GameObject obj = Instantiate(buttonPrefab, position, Quaternion.identity, Panel.transform);
        //     AbilityButton abikuButton =  obj.GetComponent<AbilityButton>();
        //     abikuButton.owningTrio = owningTrio;
        //     obj.transform.position = Panel.transform.GetChild(counter).position;
        //     abikuButton.action = abilityAction;
        //     counter++;
        //     buttons.Add(abikuButton);
        // }
    }

    void OpenUI()
    {
        Panel.SetActive(true);
    }

    void CloseUI()
    {
        Panel.SetActive(false);
    }

    void RefreshButtons()
    {
        CreateButtons();
    }

    public void OnMoveButtonClicked()
    {
        // MoveAbikuAction moveAction = owningTrio.GetMoveAction();
        // BattleLoop.Instance.SetPendingAction(moveAction);
    }

    public void onChangeAbikuButtonClicked()
    {
        // ChangeAbikuAction changeAbikuAction = owningTrio.GetCurrentAbiku().GetChangeAction();
        // BattleLoop.Instance.SetPendingAction(changeAbikuAction);
    }
}
