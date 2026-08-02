using UnityEngine;

public class WitchHex : Egungun
{
    private readonly WitchHexEgungunDefinition _config;

    public WitchHex(EgungunDefinition definition, AbikuTrio owningTrio, WitchHexEgungunDefinition config)
        : base(definition, owningTrio)
    {
        _config = config;
    }

    public override void StartTurn() => base.StartTurn();
    public override void EndTurn() => base.EndTurn();
    public override void FrameUpdate() => base.FrameUpdate();
    public override void PhysicUpdate() => base.PhysicUpdate();

    //to do later with trhread
    public int GetHexMaxCap()
    {
        //TODO
        return 3;
    }

    public int GetHexCount(GridActor target)
    {
        HexEffect hex = target.GetComponent<ActorEffectManager>().GetEffect<HexEffect>();
        if (hex != null)
        {
            return hex.CurrentStacks;
        }
        else
        {
            return 0;
        }
    }

    public void ApplyHex(GridActor target, int amount)
    {
        ActorEffectManager manager = target.GetComponent<ActorEffectManager>();
        if (manager == null) return;

        int cap = GetHexMaxCap();
        HexEffect existing = manager.GetEffect<HexEffect>();

        if (existing == null)
        {
            manager.AddEffect(new HexEffect(this, Mathf.Min(amount, cap), cap));
        }
        else
        {
            existing.Refresh(this, cap);
            existing.CurrentStacks = Mathf.Min(existing.CurrentStacks + amount, existing.MaxStacks);
        }
        
        Debug.Log(manager.GetEffect<HexEffect>().CurrentStacks);
    }
    
    public int ConsumeHex(GridActor target, int requestedAmount)
    {
        ActorEffectManager manager = target.GetComponent<ActorEffectManager>();
        if (manager == null) return 0;
        
        HexEffect hex = manager.GetEffect<HexEffect>();
        if (hex == null) return 0;

        int consumed = Mathf.Min(requestedAmount, hex.CurrentStacks);
        hex.CurrentStacks -= consumed;

        if (hex.CurrentStacks <= 0) manager.RemoveEffect(hex);
        return consumed;
    }
}