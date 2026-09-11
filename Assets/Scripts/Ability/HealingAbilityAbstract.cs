using System.Collections.Generic;
using UnityEngine;

public class HealingAbilityAbstract : AbilityAction
{
    public HealingAbilityAbstract(ISpellCaster caster, GridActor actor, AbilityTemplateSO template, int range,
        TargetingStrategySO direction, TargetTypeStrategySO targetAllowed, int numberOfTargets, ElementSO element, int turnDelay, GameCue[] AbilityThrownCues)
        : base(caster, actor, template, range, direction, targetAllowed, numberOfTargets, element)
    {
    }
    
    protected List<HealingInfo> HealTargets(IEnumerable<ITargettable> targets, int healing)
    {
        List<HealingInfo> results = new();
        foreach (var target in targets)
        {
            if (target is IDamageable damageable)
            {
                HealingInfo info = damageable.Heal(actor, null, healing, element.GetElementType());
                if(info.Target == null) continue;
                results.Add(info);
            }
        }
        CheckAndTriggerEncore(results);

        return results;
    }
    
    protected void CheckAndTriggerEncore(IEnumerable<HealingInfo> healingInfos)
    {
        foreach (HealingInfo info in healingInfos)
        {
            if (info.Target is AbikuTrio)
                continue;
            if (info.encoreTriggered)
            {
                if (info.Source is AbikuTrio trio)
                {
                    trio.TriggerEncore(info.Source);
                }
                else
                {
                    TriggerEncore(info.Target);
                }
                
                return;
            }
        }
    }
    
    protected void TriggerEncore(GridActor actor)
    {
        PlayerBattleStats.Instance.EncoreTriggered(actor);
    }
}
