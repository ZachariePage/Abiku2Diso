using System.Collections.Generic;
using UnityEngine;

public class EgungunBattleMenu : MonoBehaviour
{
    public GameObject Panel;
    public GameObject buttonPrefab;
    //horrible disgusting afront to god code that hopefully will be changed later
    private GameObject moveButton;
    private GameObject changeAbikuButton;
    
    public Egungun egungun;
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

    }

    public void onChangeAbikuButtonClicked()
    {

    }

}
