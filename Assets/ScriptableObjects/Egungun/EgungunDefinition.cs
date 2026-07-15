using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/egungun")]
public class EgungunDefinition : ScriptableObject
{
    [SerializeField] private List<AbikuStanceAbilityGroup> stanceAbilities = new List<AbikuStanceAbilityGroup>();

    private Dictionary<AbikuStanceType, List<AbilityTemplateSO>> lookup;

    //ik we building lookup each time we get abolities
    private void BuildLookup()
    {
        if (lookup != null) return;
        lookup = new Dictionary<AbikuStanceType, List<AbilityTemplateSO>>();
        foreach (var group in stanceAbilities)
        {
            lookup[group.stance] = group.abilities;
        }
        
    }

    public List<AbilityTemplateSO> GetAbilities(AbikuStanceType stance)
    {
        BuildLookup();
        if (lookup.TryGetValue(stance, out var abilities))
        {
            return abilities;
        }
        else
        {
            return new List<AbilityTemplateSO>();
        }
    }
}
