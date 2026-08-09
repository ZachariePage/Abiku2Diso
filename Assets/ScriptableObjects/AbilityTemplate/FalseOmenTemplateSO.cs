using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/FalseOmen")]
public class FalseOmenTemplateSO : AbilityTemplateSO
{
    [Header("normal class stats")]
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;
    
    [Header("ability class stats")] 
    
    [Header("Options choice")]
    public StanceOption[] availableStances;
    
    //cues
    public GameCue[] onThrownCues;
    
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new FalseOmen(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this);
    }
}
