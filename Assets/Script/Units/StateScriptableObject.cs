using UnityEngine;

public abstract class StateScriptableObject : ScriptableObject
{
    public abstract State CreateState(GridActor actor, StateMachine stateMachine);
}
