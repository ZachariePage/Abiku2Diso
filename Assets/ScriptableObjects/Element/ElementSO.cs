using UnityEngine;

[CreateAssetMenu(menuName = "Stance/Element")]
public class ElementSO : ScriptableObject
{
    private Element type;
    [SerializeField] private Element effectiveAgainst;

    public bool IsEffectiveAgainst(ElementSO defender)
    {
        return (effectiveAgainst & defender.type) != 0;
    }
    
    public Element GetEffectiveAgainst()
    {
        return effectiveAgainst;
    }

    public Element GetElementType()
    {
        return type;
    }
}
