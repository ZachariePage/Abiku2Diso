using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/egungun")]
public class EgungunDefinition : ScriptableObject
{
    [SerializeField] private List<AbikuStanceAbilityGroup> stanceAbilities = new List<AbikuStanceAbilityGroup>();

    private Dictionary<AbikuStanceType, List<AbilityTemplateSO>> lookup;

    public int HP;
    public int defense = 0;

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
    
    /*
     this version allows for fire water bitflag so stance fire and water will collect ability under fire | water
    private void BuildLookup()
    {
        if (lookup != null) return;
        lookup = new Dictionary<AbikuStanceType, List<AbilityTemplateSO>>();

        foreach (var group in stanceAbilities)
        {
            foreach (AbikuStanceType flag in Enum.GetValues(typeof(AbikuStanceType)))
            {
                if (flag == AbikuStanceType.None) continue;
                if ((group.stance & flag) == 0) continue; 

                if (!lookup.TryGetValue(flag, out var list))
                {
                    list = new List<AbilityTemplateSO>();
                    lookup[flag] = list;
                }
                list.AddRange(group.abilities);
            }
        }
    }
    */

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
