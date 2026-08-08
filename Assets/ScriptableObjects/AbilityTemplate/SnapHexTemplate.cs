using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/SnapHexTemplate")]
public class SnapHexTemplate : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Min(0f)] 
    public int numberOfHexesPerTarget;
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new SnapHex(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this);
    }
}
