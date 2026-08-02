using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Egungun
{
    [SerializeField] private EgungunDefinition definition;
    protected AbikuTrio owningTrio;

    private int HP;
    private int defense;
    public Egungun(EgungunDefinition definition, AbikuTrio owningTrio)
    {
        this.definition = definition;
        this.owningTrio = owningTrio;
        
        HP = definition.HP;
        defense = definition.defense;
    }
    
    public virtual void StartTurn()
    {
        
    }

    public virtual void EndTurn()
    {
        
    }
    
    public virtual void FrameUpdate()
    {
       
    }

    public virtual void PhysicUpdate()
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

    public int GetHP()
    {
        return HP;
    }

    public int GetDefense()
    {
        return defense;
    }
}
