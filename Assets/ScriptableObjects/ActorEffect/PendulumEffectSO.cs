using UnityEngine;


[CreateAssetMenu(menuName = "Medaillon/Pendulum")]
public class PendulumEffectSO : ActorEffectDefinition
{
    public override ActorEffect CreateEffect()
    {
        return new Pendulum(TriggerMask, Priority,StackType);
    }
}
