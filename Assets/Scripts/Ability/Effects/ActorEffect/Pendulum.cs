using UnityEngine;

public class Pendulum : ActorEffect
{
    public Pendulum(EffectTrigger triggerMask, EffectPriority priority, EffectStack stackType = EffectStack.oneMax,
        int maxStacks = 1)
        : base(triggerMask, priority, stackType, maxStacks)
    {
    }

    public override void OnApplication(GridActor self)
    {
        base.OnApplication(self);
        if (self is AbikuTrio abiku)
        {
            abiku.SetStanceLocked(true);
        }
    }

    public override void OnAbilityFinished(GridActor self, AbilityAftermathInfo info)
    {
        base.OnAbilityFinished(self, info);
        if (info.UsedAction.GetActionType() != BattleActionType.ability) return;
        if (self is not AbikuTrio abiku) return;

        abiku.ChangeStance();
        Debug.LogWarning("finish pendulum discount");
        //abiku.GrantNextSkillDiscount(config.DiscountAmount);
    }

    public override void OnRemoval(GridActor self)
    {
        base.OnRemoval(self);
        if (self is AbikuTrio abiku)
        {
            abiku.SetStanceLocked(false);
        }
    }
}
