using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Egungun
{
    [SerializeField] private EgungunDefinition definition;
    private AbikuTrio owningTrio;
    
    public Egungun(EgungunDefinition definition, AbikuTrio owningTrio)
    {
        this.definition = definition;
        this.owningTrio = owningTrio;
    }
    
    public void StartTurn()
    {
        
    }

    public void EndTurn()
    {
        
    }
    
    public void FrameUpdate()
    {
       
    }

    public void PhysicUpdate()
    {
        
    }

    public AbikuTrio GetOwningTrio()
    {
        return owningTrio;
    }

    public void SetOwningTrio(AbikuTrio owningTrio)
    {
        this.owningTrio = owningTrio;
    }
    
    public List<AbilityTemplateSO> GetAbilitiesForStance(AbikuStanceType stance)
    {
        return definition.GetAbilities(stance);
    }
}
