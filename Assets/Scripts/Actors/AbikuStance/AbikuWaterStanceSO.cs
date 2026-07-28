using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Abiku/waterStanceSO")]
public class AbikuWaterStanceSO : AbikuStanceScriptableObject
{
    public override State CreateState(GridActor actor, IStateMachine stateMachine)
    {
        return null;
    }

    public override AbikuStance CreateAbikuStanceState(GridActor actor, IStateMachine stateMachine)
    {
        return new AbikuWaterStance(actor, stateMachine, this);
    }
}
