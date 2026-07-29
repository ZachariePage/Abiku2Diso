using UnityEngine;

[CreateAssetMenu(menuName = "Medaillon/Interpose")]
public class InterposeEffectSO : ActorEffectDefinition
{
    public override ActorEffect CreateEffect()
    {
        return new InterposeEffect(TriggerMask, Priority,StackType, this);
    }
}
