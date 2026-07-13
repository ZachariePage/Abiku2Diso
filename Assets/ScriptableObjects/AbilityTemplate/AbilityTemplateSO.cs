using UnityEngine;

public abstract class AbilityTemplateSO : ScriptableObject
{
    public abstract AbilityAction CreateAction(GridActor owner);
}
