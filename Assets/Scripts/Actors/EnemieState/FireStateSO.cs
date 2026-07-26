using UnityEngine;

[CreateAssetMenu(menuName = "State/Enemy/firestateSO")]
public class FireStateSO : EnemyStanceScriptableObject
{
    public AbilityTemplateSO action;
    public ElementSO element;
    public override State CreateState(GridActor actor, IStateMachine stateMachine)
    {
        return null;
    }

    public override EnemyStance CreateEnemyState(GridActor actor, IStateMachine stateMachine)
    {
        return new FireState(actor, stateMachine, this);
    }

    // public override AbilityTemplateSO GetAction()
    // {
    //     return action;
    // }
}
