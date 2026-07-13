using UnityEngine;
[CreateAssetMenu(menuName = "State/waterStateSO")]
public class WaterStanceSO : StanceStateScriptableObject
{
    public AbilityTemplateSO action;
    public ElementSO element;
    public override State CreateState(GridActor actor, StateMachine stateMachine)
    {
        return new WaterState(actor, stateMachine, this);
    }

    public override AbilityTemplateSO GetAction()
    {
        return action;
    }
}

