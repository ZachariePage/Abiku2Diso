using UnityEngine;

public abstract class AbilityTemplateSO : ScriptableObject
{
    public string DisplayName;
    public abstract AbilityAction CreateAction(ISpellCaster caster, GridActor owner);
}
