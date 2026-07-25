using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/Fireball")]
public class FireballTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    //cues
    public GameCue[] onThrownCues;

    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new Fireball(caster, owner, range, targetingStrategy, targetTypeStrategy, numberOfTargets, element, onThrownCues);
    }
}
