using UnityEngine;

[CreateAssetMenu(menuName = "Medaillon/CrossStep")]
public class CrossStepEffectSO : ActorEffectDefinition
{
    public override ActorEffect CreateEffect()
    {
        return new CrossStep(TriggerMask, Priority,this);
    }
}
