using UnityEngine;

public enum BoulderType
{
    light,
    heavy
}
public abstract class Boulder : GridActor, IDamageable
{
    private Health health;
    protected BoulderType type;
    public GridCell HoldingCell;
    public ShieldBreaker Owner;
    protected int HitsTaken;

    protected override void Start()
    {
        if (GetComponent<Health>() == null)
        {
            gameObject.AddComponent<Health>();
        }
        base.Start();
    }

    public DamageInfo TakeDamage(GridActor source, IDamageSource damageSource, float damage, Element element)
    {
        DamageInfo info = new DamageInfo(source, this, damageSource, damage, element, Element.None, false);
        return info;
    }

    public HealingInfo Heal(GridActor source, IHealingSource healingSource, float heal, Element element)
    {
        HealingInfo info = new HealingInfo(source, this, healingSource, heal, element, Element.None, false);
        return info;
    }
    

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        return null;
    }

    public bool IsWounded()
    {
        return health.IsWounded();
    }

    public Health GetHealth()
    {
        return health;
    }
    
    public abstract void OnPushed(BoulderManager manager, GridActor pusher, Vector2Int direction);

    public virtual void OnExplode(BoulderManager manager)
    {
        
    }
}
