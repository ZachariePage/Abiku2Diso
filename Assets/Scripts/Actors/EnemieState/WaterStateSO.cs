using UnityEngine;
[CreateAssetMenu(menuName = "State/Enemy/waterStateSO")]
public class WaterStanceSO : EnemyStanceScriptableObject
{
    public AbilityTemplateSO action;
    public ElementSO element;

    public override EnemyStance CreateEnemyState(GridActor actor, IStateMachine stateMachine)
    {
        return new WaterState(actor, stateMachine, this);
    }

    // public override AbilityTemplateSO GetAction()
    // {
    //     return action;
    // }
    public override State CreateState(GridActor actor, IStateMachine stateMachine)
    {
        return null;
    }
}

