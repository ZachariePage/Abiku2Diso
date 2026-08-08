using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/ResonantChainTemplate")]
public class ResonantChainTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Min(0f)] 
    public float damage;
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new ResonantChain(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this, damage);
    }
}

