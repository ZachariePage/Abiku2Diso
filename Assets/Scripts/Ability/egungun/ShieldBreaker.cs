using UnityEngine;

public class ShieldBreaker : Egungun
{
    private ShieldBreakerEgungunDefinition _config;
    public BoulderManager BoulderManager;
    public ShieldBreaker(EgungunDefinition definition, AbikuTrio owningTrio, ShieldBreakerEgungunDefinition config) : base(definition, owningTrio)
    {
        _config = config;
        BoulderManager = new BoulderManager(this, config);
    }
    
    public override void StartTurn() => base.StartTurn();
    public override void EndTurn() => base.EndTurn();
    public override void FrameUpdate() => base.FrameUpdate();
    public override void PhysicUpdate() => base.PhysicUpdate();
}
