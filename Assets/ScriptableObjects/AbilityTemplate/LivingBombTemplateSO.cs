using UnityEngine;


[CreateAssetMenu(menuName = "Abilty/LivingBomb")]
public class LivingBombTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;
    public float damage;

    //cues
    public GameCue[] onThrownCues;

    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new LivingBomb(caster, owner,this, range, targetingStrategy, targetTypeStrategy, numberOfTargets, element, this);
    }
}
