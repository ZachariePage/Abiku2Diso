using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbikuStanceScriptableObject : StateScriptableObject
{
    public abstract AbikuStance CreateAbikuStanceState(GridActor actor, IStateMachine stateMachine);
}
