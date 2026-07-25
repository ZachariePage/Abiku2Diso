using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/DelayedAttacl")]
public class DelayedAttackTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public int range;
    public ElementSO element;

    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new DelayedAttack(caster, owner, range, targetingStrategy,targetTypeStrategy, 0, element);
    }
}