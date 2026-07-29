using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingAbility : AbilityAction
{
    protected DamagingAbility(ISpellCaster caster, GridActor actor,AbilityTemplateSO template, int range, TargetingStrategySO direction,
        TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element)
        : base(caster, actor,template, range, direction, targetAllowed, numberOfTargets, element)
    {
    }

    protected List<DamageInfo> DealDamageToTargets(IEnumerable<ITargettable> targets, int damage)
    {
        List<DamageInfo> results = new();
        foreach (var target in targets)
        {
            if (target is IDamageable damageable)
            {
                float finalDamage = damageable.ModifyIncomingDamage(damage);
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
                    trio.TriggerEncore();
                }
                else
                {
                    TriggerEncore();
                }
                
                return;
            }
        }
    }
    
    protected void TriggerEncore()
    {
        PlayerBattleStats.Instance.EncoreTriggered();
    }
}
