using System;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour, IHoverable
{
    public AbikuTrio owningTrio;
    public BattleAction action;
    public IReadOnlyBattleAction readOnlyAction;
    
    private HoverableUIData hoverData = new HoverableUIData();
    public void OnClick()
    {
        if (!action.ReadyToUse())
        {
            return;
        }
        BattleLoop.Instance.SetPendingAction(action);
    }

    public void Init()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = action.ToString();
        
        hoverData.name = action.ToString();
        readOnlyAction = action;
    }

    public HoverableUIData GetHoverData()
    {
        return action.GetHoverData();
    }

    public HoverableUIType GetHoverType()
    {
        return HoverableUIType.AbilityDescription;
    }
}
