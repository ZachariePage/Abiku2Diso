using System;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Element
{
    None  = 0,
    Fire  = 1 << 0,
    Water = 1 << 1,
    Wood  = 1 << 2,
}


public class ElementSystem : MonoBehaviour
{
    public bool IsEffectiveAgainst(ElementSO dealing, ElementSO defender)
    {
        return (dealing.GetEffectiveAgainst() & defender.GetElementType()) != 0;
    }
}
