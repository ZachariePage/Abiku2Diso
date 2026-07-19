using UnityEngine;

public struct DamageInfo
{
    public GridActor Source;
    public GridActor Target;
    public AbilityAction abilityUsed;
    public float finalDamage;
    public Element elementUsed;
    public Element elementAgainst;
    public bool encoreTriggered;

    public DamageInfo(GridActor source, GridActor target, AbilityAction abilityUsed, float finalDamage, Element elementUsed, Element elementAgainst, bool encoreTriggered)
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
    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, Element element);
}
