using UnityEngine;

public abstract class AbilityTemplateSO : ScriptableObject
{
    public string DisplayName;
    public int manaCost;
    public abstract AbilityAction CreateAction(ISpellCaster caster, GridActor owner);
}
