using UnityEngine;

[CreateAssetMenu(menuName = "Abilty/ExplosionWitchHexSO")]
public class ExplosionWitchHexTemplateSO : AbilityTemplateSO
{
    public TargetingStrategySO targetingStrategy;
    public TargetTypeStrategySO targetTypeStrategy;
    public ElementSO element;
    public int range;
    public int numberOfTargets;

    public bool HurtAllies;
    [Min(0f)] 
    public int damage;
    [Min(0f)] 
    public int damagePerHex;
    [Min(0f)] 
    public int RangeExplosion;
    [Min(0f)]
    public int RangeExplosionPerHex; 
    
    //cues
    public GameCue[] onThrownCues;
    public override AbilityAction CreateAction(ISpellCaster caster, GridActor owner)
    {
        return new ExplosionWitchHex(caster, owner, this, range, targetingStrategy, targetTypeStrategy, numberOfTargets,
            element, this);
    }
}

