using UnityEngine;

public class InterposeEffect : ActorEffect
{
    public InterposeEffectSO config;
    public InterposeEffect(EffectTrigger triggerMask, EffectPriority priority,EffectStack stackType, InterposeEffectSO config) : base(triggerMask, priority,stackType)
    {
        this.config = config;
    }

    public override void OnAbilityFinished(GridActor self, AbilityAftermathInfo info)
    {
        base.OnAbilityFinished(self, info);
        ITargettable firsttarget = info.targets[0];
        if (firsttarget == null) return;
    
        GridActor target = firsttarget.GetActor();
        if (target == null) return;
    
        if(target.GetMyTeam() != self.GetMyTeam()) return;
        Debug.Log("interposing");
        
        GridCell myCell = self.GetHoldingCell();
        GridCell targetCell = target.GetHoldingCell();
        
        self.MoveToCell(targetCell);
        target.MoveToCell(myCell);
    }
}
