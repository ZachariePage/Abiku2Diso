using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/DelayedAttacl")]
public class DelayedAttackTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public int range;
    public ElementSO element;

    public override AbilityAction CreateAction(GridActor owner)
    {
        return new DelayedAttack(owner, range, targetingStrategy.allowedDirections,TargetType.EmptyCell, 0, element);
    }
}