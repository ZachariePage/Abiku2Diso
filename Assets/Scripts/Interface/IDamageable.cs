using UnityEngine;

public struct DamageInfo
{
    public GridActor Source;
    public GridActor Target;
    public AbilityAction abilityUsed;
    public float finalDamage;
    public ElementSO elementUsed;
    public ElementSO elementAgainst;
    public bool encoreTriggered;

    public DamageInfo(GridActor source, GridActor target, AbilityAction abilityUsed, float finalDamage, ElementSO elementUsed, ElementSO elementAgainst, bool encoreTriggered)
    {
        Source = source;
        Target = target;
        this.abilityUsed = abilityUsed;
        this.finalDamage = finalDamage;
        this.elementUsed = elementUsed;
        this.elementAgainst = elementAgainst;
        this.encoreTriggered = encoreTriggered;
    }
}
public interface IDamageable
{
    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, ElementSO element);
}
