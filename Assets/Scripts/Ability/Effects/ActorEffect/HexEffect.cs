using UnityEngine;

public class HexEffect : ActorEffect
{
    public WitchHex applierWitch;

    public WitchHex GetApplierActor()
    {
        return applierWitch;
    }

    public HexEffect(WitchHex applierWitch, int initialStacks, int maxStacks)
        : base(EffectTrigger.OnDamageTaken, EffectPriority.Second, EffectStack.definedAmount, maxStacks)
    {
        this.applierWitch = applierWitch;
        CurrentStacks = Mathf.Min(initialStacks, maxStacks);
    }
    
    public void Refresh(WitchHex newApplier, int newMaxCap)
    {
        applierWitch = newApplier;
        MaxStacks = Mathf.Max(MaxStacks, newMaxCap);
    }

    public override void OnDamageTaken(GridActor self, DamageInfo info)
    {
        base.OnDamageTaken(self, info);

        GridActor applierTrio = applierWitch.GetOwningTrio();
        //if (witch.GetThreadLevel() < 2) return;
        if (self.GetMyTeam() != applierTrio.GetMyTeam()) return; 

        //witch.ApplyHex(info.Source, 1);
    }

    public override void OnRemoval(GridActor self)
    {
        base.OnRemoval(self);
        applierWitch.RemoveHex(self);
    }
}
