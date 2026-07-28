using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/Fireball")]
public class FireballTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;
    public int turnDelay = 2;

    //cues
    public GameCue[] onThrownCues;

    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new Fireball(caster, owner,this, range, targetingStrategy, targetTypeStrategy, numberOfTargets, element,turnDelay, onThrownCues);
    }
}
