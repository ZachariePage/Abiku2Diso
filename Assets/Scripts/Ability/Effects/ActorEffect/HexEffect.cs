using UnityEngine;

public class HexEffect : ActorEffect
{
    public WitchHex ApplierWitch;

    public WitchHex GetApplierActor()
    {
        return ApplierWitch;
    }

    public HexEffect(WitchHex applierActor, int initialStacks, int maxStacks)
        : base(EffectTrigger.OnDamageTaken, EffectPriority.Second, EffectStack.definedAmount, maxStacks)
    {
        ApplierWitch = applierActor;
        CurrentStacks = Mathf.Min(initialStacks, maxStacks);
    }
    
    public void Refresh(WitchHex newApplier, int newMaxCap)
    {
        ApplierWitch = newApplier;
        MaxStacks = Mathf.Max(MaxStacks, newMaxCap);
    }

    public override void OnDamageTaken(GridActor self, DamageInfo info)
    {
        base.OnDamageTaken(self, info);

        GridActor applierTrio = ApplierWitch.GetOwningTrio();
        //if (witch.GetThreadLevel() < 2) return;
        if (self.GetMyTeam() != applierTrio.GetMyTeam()) return; 

        //witch.ApplyHex(info.Source, 1);
    }

    public override void OnRemoval(GridActor self)
    {
        base.OnRemoval(self);
    }
}
