using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingBattleEffect : BattleEffect
{
    protected ElementSO _element;
    protected GridActor actor;
    
    //I SHOULD REALLY MUCH DO SOMETHING FOR THE INFO ABILITY SO IT ACCEPT EFFECT AS WELL
    protected DamagingBattleEffect(ElementSO element)
    {
        _element = element;
    }

    protected List<DamageInfo> DealDamageToTargets(IEnumerable<ITargettable> targets, int damage)
    {
        List<DamageInfo> results = new();
        foreach (var target in targets)
        {
            if (target is IDamageable damageable)
            {
                DamageInfo info = damageable.TakeDamage(actor, null, damage, _element.GetElementType());
                if(info.Target != null) continue;
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
                TriggerEncore();
                return;
            }
        }
    }
    
    protected void TriggerEncore()
    {
        PlayerBattleStats.Instance.EncoreTriggered();
    }
}
