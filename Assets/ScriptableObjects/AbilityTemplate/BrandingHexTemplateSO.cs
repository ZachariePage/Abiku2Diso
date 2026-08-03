using UnityEngine;


[CreateAssetMenu(menuName = "Abilty/BrandingHex")]
public class BrandingHexTemplateSO : AbilityTemplateSO
{
    [Header("normal class stats")]
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Header("ability class stats")] 
    public int normalHexAmount;
    public int advantageHexAmount;
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new BrandingHexAction(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this, normalHexAmount,  advantageHexAmount);
    }
}
