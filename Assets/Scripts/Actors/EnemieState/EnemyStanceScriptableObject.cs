using UnityEngine;

public abstract class EnemyStanceScriptableObject : StateScriptableObject
{
    public Sprite icon;
    public abstract EnemyStance CreateEnemyState(GridActor actor, IStateMachine stateMachine);
}
