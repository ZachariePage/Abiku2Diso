using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanceBattleMenu : MonoBehaviour
{
    public GameObject Panel;
    public GameObject buttonPrefab;

    [SerializeField] private Image stanceIcon;
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
        owningTrio.onChangeStance += StanceIcon;
        Panel.SetActive(false);
        
        if (owningTrio.StanceStateMachine.CurrentState != stance)
        {
            stanceIcon.gameObject.SetActive(false);
        }
        
        CreateButtons();
        CreateIcon();
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
            abikuButton.Init();
            counter++;
            buttons.Add(abikuButton);
        }
    }

    public void CreateIcon()
    {
        stanceIcon.sprite = stance.GetStanceConfig().icon;
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

    void StanceIcon()
    {
        if(owningTrio.StanceStateMachine.CurrentState != stance)
        {
            stanceIcon.gameObject.SetActive(false);
        }
        else
        {
            stanceIcon.gameObject.SetActive(true);
        }
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
