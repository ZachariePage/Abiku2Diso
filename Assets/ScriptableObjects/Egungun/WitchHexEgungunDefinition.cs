using UnityEngine;

[CreateAssetMenu(menuName = "Unit/WitchHexEgungunDefinition")]
public class WitchHexEgungunDefinition : EgungunDefinition
{
    public override Egungun CreateEgungun(EgungunDefinition definition, AbikuTrio owningTrio)
    {
        return new WitchHex(definition, owningTrio, this);
    }
}
