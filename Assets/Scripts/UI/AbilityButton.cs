using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public AbikuTrio owningTrio;
    public BattleAction action;
    
    public void OnClick()
    {
        BattleLoop.Instance.SetPendingAction(action);
    }
}
