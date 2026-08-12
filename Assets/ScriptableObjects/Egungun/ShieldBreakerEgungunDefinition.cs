using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ShieldBrokerEgungunDefinition")]
public class ShieldBreakerEgungunDefinition : EgungunDefinition
{
    public GameObject lightBoulderPrefab;
    public GameObject heavyBoulderPrefab;

    public int maxHeavyBoulders;
    public int maxLightBoulders;
    public override Egungun CreateEgungun(EgungunDefinition definition, AbikuTrio owningTrio)
    {
        return new ShieldBreaker(definition, owningTrio, this);
    }
}
