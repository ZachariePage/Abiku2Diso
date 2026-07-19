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
    public static ElementSystem Instance;
    [System.Serializable]
    private struct Matchup
    {
        public Element element;
        public Element effectiveAgainst;
    }

    [SerializeField] private Matchup[] matchups;
    private Dictionary<Element, Element> _lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _lookup = new Dictionary<Element, Element>();
        foreach (var m in matchups)
        {
            _lookup[m.element] = m.effectiveAgainst;
        }
    }

    public bool IsEffectiveAgainst(Element attacker, Element defender)
    {
        return _lookup.TryGetValue(attacker, out var mask) && (mask & defender) != 0;
    }
}
