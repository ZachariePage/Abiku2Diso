using UnityEngine;

[CreateAssetMenu(menuName = "State/Enemy/firestateSO")]
public class FireStateSO : StanceStateScriptableObject
{
    public AbilityTemplateSO action;
    public ElementSO element;
    public override State CreateState(GridActor actor, StateMachine stateMachine)
    {
        return new FireState(actor, stateMachine, this);
    }

    public override AbilityTemplateSO GetAction()
    {
        return action;
    }
}
