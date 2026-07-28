using System;
using UnityEngine;

public enum HoverableUIType
{
    UnitDescription,
    AbilityDescription,
    DamageAssesment
}
[Serializable]
public struct HoverableUIData
{
    public string name;
    public string description;

    public HoverableUIData(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
public interface IHoverable 
{
    public HoverableUIData GetHoverData();
    public HoverableUIType GetHoverType();
}
