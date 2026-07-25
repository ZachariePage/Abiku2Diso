using System;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public AbikuTrio owningTrio;
    public BattleAction action;
    
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
    }
}
