using UnityEngine;

public interface IDamageSource
{
    string DisplayName { get; } 
    GridActor SourceActor { get; }
}

public interface IHealingSource
{
    string DisplayName { get; } 
    GridActor SourceActor { get; }
}
public class DamageProposalContext
{
    public GridActor Self;
    public GridActor Source;
    public Element DamageElement;
    public float DamageProposed;
}

public class DamageMitigationContext
{
    public GridActor Self;
    public GridActor Source;
    public Element DamageElement;

    public float IncomingDamage;
    public int Defense;
    public bool Dodged;
}
public struct DamageInfo
{
    public GridActor Source;
    public GridActor Target;
    public IDamageSource damageSource;
    public float finalDamage;
    public Element elementUsed;
    public Element elementAgainst;
    public bool encoreTriggered;

    public DamageInfo(GridActor source, GridActor target, IDamageSource damageSource, float finalDamage, Element elementUsed, Element elementAgainst, bool encoreTriggered)
    {
        Source = source;
        Target = target;
        this.damageSource = damageSource;
        this.finalDamage = finalDamage;
        this.elementUsed = elementUsed;
        this.elementAgainst = elementAgainst;
        this.encoreTriggered = encoreTriggered;
    }
}

public struct HealingInfo
{
    public GridActor Source;
    public GridActor Target;
    public IHealingSource healingSource;
    public float finalHeal;
    public Element elementUsed;
    public Element elementAgainst;
    public bool encoreTriggered;

    public HealingInfo(GridActor source, GridActor target, IHealingSource healingSource, float finalHeal, Element elementUsed, Element elementAgainst, bool encoreTriggered)
    {
        Source = source;
        Target = target;
        this.healingSource = healingSource;
        this.finalHeal = finalHeal;
        this.elementUsed = elementUsed;
        this.elementAgainst = elementAgainst;
        this.encoreTriggered = encoreTriggered;
    }
}
public interface IDamageable
{
    public DamageInfo TakeDamage(GridActor source, IDamageSource damageSource, float damage, Element element);
    public HealingInfo Heal(GridActor source, IHealingSource healingSource, float heal, Element element);

    public void BuffDefense(int value);

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx);

    public bool IsWounded();
    
    public Health GetHealth();
}
