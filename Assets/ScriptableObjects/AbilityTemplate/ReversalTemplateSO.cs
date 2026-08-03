using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/ReversalTemplate")]
public class ReversalTemplateSO : AbilityTemplateSO
{
    [Header("normal class stats")]
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Header("ability class stats")] 
    [Range(0, 100)]
    public int reduceDamagePercentage;
    [Range(0, 100)]
    public int healPercentage;
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new Reversal(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this, reduceDamagePercentage,  healPercentage);
    }
}
