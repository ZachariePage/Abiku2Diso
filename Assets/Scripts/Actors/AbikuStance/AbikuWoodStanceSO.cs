using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Abiku/woodStanceSO")]
public class AbikuWoodStanceSO : AbikuStanceScriptableObject
{
    public ElementSO element;
    public override State CreateState(GridActor actor, StateMachine stateMachine)
    {
        return null;
    }

    public override AbikuStance CreateAbikuStanceState(GridActor actor, StateMachine stateMachine)
    {
        return new AbikuWoodStance(actor, stateMachine, this);
    }
}
