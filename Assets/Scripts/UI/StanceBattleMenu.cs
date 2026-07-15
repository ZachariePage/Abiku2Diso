using System.Collections.Generic;
using UnityEngine;

public class StanceBattleMenu : MonoBehaviour
{
    public GameObject Panel;
    public GameObject buttonPrefab;
    //horrible disgusting afront to god code that hopefully will be changed later
    private GameObject moveButton;
    private GameObject changeAbikuButton;
    
    public AbikuStance stance;
    public AbikuTrio owningTrio;
    private List<AbilityButton> buttons = new List<AbilityButton>();
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
        buttons.Clear();
        int counter = 0;
        
        foreach (BattleAction abilityAction in stance.GetAbilities())
        {
            Vector3 position = Panel.transform.GetChild(counter).position;
            GameObject obj = Instantiate(buttonPrefab, position, Quaternion.identity, Panel.transform);
            AbilityButton abikuButton =  obj.GetComponent<AbilityButton>();
            abikuButton.owningTrio = owningTrio;
            obj.transform.position = Panel.transform.GetChild(counter).position;
            abikuButton.action = abilityAction;
            counter++;
            buttons.Add(abikuButton);
        }
    }

    void OpenUI()
    {
        if(owningTrio.StanceStateMachine.CurrentState != stance) return;
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

    }

    public void onChangeAbikuButtonClicked()
    {

    }

}
