using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class Abiku
{
    [SerializeField] private AbikuDefinition abikuDefinition;
    
    [Header("stats")]
    public float health;
    public float damage;
    public AbikuTrio owningTrio;

    private MoveAbikuAction moveAction;
    private ChangeAbikuAction changeAction;
    
    [SerializeField] private List<AbilityAction> abilityActions =  new List<AbilityAction>();

    public Abiku(AbikuDefinition abikuDefinition, float health, float damage, AbikuTrio owningTrio)
    {
        this.abikuDefinition = abikuDefinition;
        this.health = health;
        this.damage = damage;
        this.owningTrio = owningTrio;
    }

    public void Initialize()
    {
        // moveAction = new MoveAbikuAction(this);
        // changeAction = new ChangeAbikuAction(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //getters setters
    public AbikuDefinition GetAbikuDefinition()
    {
        return abikuDefinition;
    }

    public void SetAbikuDefinition(AbikuDefinition newDefinition)
    {
        this.abikuDefinition = newDefinition;
    }

    public void AddAbility(AbilityAction abilityAction)
    {
        abilityActions.Add(abilityAction);
    }

    public AbilityAction GetAbilityAction(int abilityIndex)
    {
        return abilityActions[abilityIndex];
    }

    public MoveAbikuAction GetMoveAction()
    {
        return moveAction;
    }

    public ChangeAbikuAction GetChangeAction()
    {
        return changeAction;
    }
}
