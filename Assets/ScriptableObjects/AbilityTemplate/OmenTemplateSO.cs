using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/OmenTemplate")]
public class OmenTemplateSO : AbilityTemplateSO
{
    [Header("normal class stats")]
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    [Header("ability class stats")]
    [Min(0f)] 
    public int speedBuffFlatAmount;
    [Min(0f)] 
    public int speedDebuffFlatAmount;
    [Min(0f)] 
    public int defenseBuffFlatAmount;
    [Min(0f)] 
    public int defenseDebuffFlatAmount;
    [Min(0f)] 
    public int damageBuffFlatAmount;
    [Min(0f)] 
    public int damageDebuffFlatAmount;
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new Omen(this, caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element);
    }
}
