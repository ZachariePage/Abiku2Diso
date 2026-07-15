using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridActor,  IDamageable
{
    [Header("state machie")]
    public StateMachine<State> StateMachine;

    public StanceStateScriptableObject[] startingState;
    private List<State> states = new List<State>();
    private int currentStateIndex = 0;
    
    private float health;
    private Element currentElement;

    public int turnBeforeExecutingAction = -1;
    
    [SerializeField] private List<AbilityAction> abilityActions =  new List<AbilityAction>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StateMachine = new StateMachine<State>();
        if (startingState.Length == 0)
        {
            Debug.LogError("THERE ARE NO STARTING STATE REEEEEEEEEEEEE");
        }
        foreach (var stateSO in startingState)
        {
            State state = stateSO.CreateState(this, StateMachine);
            states.Add(state);
        }
        
        StateMachine.Init(states[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        
    }
    
    public override void Highlight(CellHighlightState mode)
    {
        base.Highlight(mode);
        Debug.Log("i am highlight");
        GetHoldingCell().Highlight(mode);
    }

    public override void UnHighlight()
    {
        base.UnHighlight();
        GetHoldingCell().UnHighlight();
    }
    
    public IEnumerator TakeTurn()
    {
        Debug.Log("i am take turn");
        
        Debug.Log(StateMachine.CurrentState);
        StateMachine.CurrentState.StartTurn();
        
        yield return new WaitForSeconds(1f);
        
        StateMachine.CurrentState.EndTurn();
        
        Debug.Log("turn finished");
    }

    public void ExecuteAction(AbilityAction action, Action onActionFinished)
    {
        StartCoroutine(action.Execute(onActionFinished));
    }

    public void ChangeStateThroughIncrementation()
    {
        currentStateIndex = (currentStateIndex + 1) % states.Count;
        StateMachine.ChangeState(states[currentStateIndex]);
    }


    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, ElementSO element)
    {
        health -= damage;
        Debug.Log(health);
        return new DamageInfo();
    }
    
    //getter setter add
    public void AddAbility(AbilityAction abilityAction)
    {
        abilityActions.Add(abilityAction);
    }

    public void SetElement(Element element)
    {
        currentElement = element;
    }

    public Element getElement()
    {
        return currentElement;
    }
}
