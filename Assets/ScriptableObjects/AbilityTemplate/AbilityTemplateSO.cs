using System;
using UnityEngine;

[Flags]
public enum AbilityTag
{
    none = 0,
    Offensive = 1 << 0,
    Support = 1 << 1,
    Movement  = 1 << 2,
    Protection  = 1 << 3,
    Control = 1 << 4,
    Zone = 1 << 5,
    Counter = 1 << 6,
    Encore =  1 << 7,
}
public abstract class AbilityTemplateSO : ScriptableObject
{
    [Header("Parent class stats")]
    public string DisplayName;
    public int manaCost;
    public HoverableUIData hoverData;
    public AbilityTag abilityGenre;
    public abstract AbilityAction CreateAction(ISpellCaster caster, GridActor owner);
}
