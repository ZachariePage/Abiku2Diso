using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/StandingWallSO")]
public class StandingWallTemplateSO : AbilityTemplateSO
{
    [Header("normal class stats")]
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Header("ability class stats")]
    public BoulderType typeToSpawn = BoulderType.heavy;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new StandingWall(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this);
    }
}
