using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/heal spell")]
public class HealingTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;
    public int turnDelay = 2;
    
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new HealingAbility(caster, owner,this, range, targetingStrategy, targetTypeStrategy, numberOfTargets, element,turnDelay, onThrownCues);
    }
}
