using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DamagingAbility : AbilityAction, IDamageSource
{
    protected DamagingAbility(ISpellCaster caster, GridActor actor,AbilityTemplateSO template, int range, TargetingStrategySO direction,
        TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
        : base(caster, actor,template, range, direction, targetAllowed, numberOfTargets, element)
    {
    }

    protected List<DamageInfo> DealDamageToTargets(IEnumerable<ITargettable> targets, float damage)
    {
        List<DamageInfo> results = new();
        foreach (ITargettable target in targets.ToArray())
        {
            if (target is IDamageable damageable)
            {
                DamageProposalContext ctx = new DamageProposalContext
                {
                    Self = target.GetActor(),
                    Source = actor,
                    DamageElement = element.GetElementType(),
                    DamageProposed = damage
                };

                actor.GetComponent<IDamageable>().ModifyOutgoingDamage(ctx);
                float finalDamage = ctx.DamageProposed;
                
                DamageInfo info = damageable.TakeDamage(actor, null, finalDamage, element.GetElementType());
                if(info.Target == null) continue;
                results.Add(info);
            }
        }
        CheckAndTriggerEncore(results);

        return results;
    }
    
    protected void CheckAndTriggerEncore(IEnumerable<DamageInfo> damageInfos)
    {
        foreach (DamageInfo damageInfo in damageInfos)
        {
            if (damageInfo.Target is AbikuTrio)
                continue;
            if (damageInfo.encoreTriggered)
            {
                if (damageInfo.Source is AbikuTrio trio)
                {
                    trio.TriggerEncore(damageInfo.Target);
                }
                else
                {
                    TriggerEncore(damageInfo.Target);
                }
                
                return;
            }
        }
    }
    
    protected void TriggerEncore(GridActor actor)
    {
        PlayerBattleStats.Instance.EncoreTriggered(actor);
    }

    public string DisplayName => template.DisplayName;
    public GridActor SourceActor => GetActorOwner();
}
