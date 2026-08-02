using UnityEngine;


[CreateAssetMenu(menuName = "Abilty/BrandingHex")]
public class BrandingHexTemplateSO : AbilityTemplateSO
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
        return new BrandingHexAction(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this);
    }
}
