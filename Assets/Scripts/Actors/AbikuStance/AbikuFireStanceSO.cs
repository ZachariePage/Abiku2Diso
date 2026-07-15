using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Abiku/fireStanceSO")]
public class AbikuFireStanceSO : AbikuStanceScriptableObject
{
    public ElementSO element;

    public override State CreateState(GridActor actor, StateMachine stateMachine)
    {
        return null;
    }

    public override AbikuStance CreateAbikuStanceState(GridActor actor, StateMachine stateMachine)
    {
        return new AbikuFireStance(actor, stateMachine, this);
    }
}
