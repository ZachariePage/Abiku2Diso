using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/Fireball")]
public class FireballTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;


    public override AbilityAction CreateAction(GridActor owner)
    {
        return new Fireball(owner, range, targetingStrategy.allowedDirections, targetTypeStrategy.allowedTarget, numberOfTargets, element);
    }
}
