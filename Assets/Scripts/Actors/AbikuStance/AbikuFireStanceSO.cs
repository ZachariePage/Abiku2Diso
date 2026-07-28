using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Abiku/fireStanceSO")]
public class AbikuFireStanceSO : AbikuStanceScriptableObject
{
    public override State CreateState(GridActor actor, IStateMachine stateMachine)
    {
        return null;
    }

    public override AbikuStance CreateAbikuStanceState(GridActor actor, IStateMachine stateMachine)
    {
        return new AbikuFireStance(actor, stateMachine, this);
    }
}
