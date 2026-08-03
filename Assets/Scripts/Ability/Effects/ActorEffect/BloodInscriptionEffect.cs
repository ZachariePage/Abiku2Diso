using Unity.Mathematics;
using UnityEngine;

public class BloodInscriptionEffect : ActorEffect
{
    private int CurrentBankedStacks = 0;
    public BloodInscriptionEffect(int bankedStacks)
        : base(EffectTrigger.OnDamageTaken, EffectPriority.First, EffectStack.oneMax, 1)
    {
        CurrentBankedStacks = bankedStacks;
    }

    public override void OnApplication(GridActor self)
    {
        base.OnApplication(self);
    }

    public override void OnDamageTaken(GridActor self, DamageInfo info)
    {
        base.OnDamageTaken(self, info);
        if (self is not AbikuTrio actor ||
            actor.GetEgungun() is not WitchHex witch)
        {
            Debug.LogError("Why is this spell not on a witch?");
            self.GetComponent<ActorEffectManager>().RemoveEffect(this);
            return;
        }

        if (info.Source == null)
        {
            Debug.LogError("The attacker is null for blood inscription effect.");
            return;
        }
        HexEffect witchHex = witch.GetHexedEffect(self);
        if (witchHex == null)
        {
            self.GetComponent<ActorEffectManager>().RemoveEffect(this);
            Debug.Log("no HexEffect found somehow");
            return;
        }
        int numberOfHexToTransfer = math.min(witchHex.CurrentStacks, CurrentBankedStacks);
        witch.ConsumeHex(self,  numberOfHexToTransfer);
        witch.ApplyHex(info.Source, numberOfHexToTransfer);
        
        self.GetComponent<ActorEffectManager>().RemoveEffect(this);
    }
}
