using UnityEngine;

[CreateAssetMenu(menuName = "Medaillon/CloseWeave")]
public class CloseWeaveSO : ActorEffectDefinition
{
    public override ActorEffect CreateEffect()
    {
        return new CloseWeave(TriggerMask, Priority, StackType,this);
    }
}
