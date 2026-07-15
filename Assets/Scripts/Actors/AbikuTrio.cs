using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbikuTrio : GridActor,  IDamageable
{
    [SerializeField] private TrioDefinition trioDefinition;
    // [SerializeField] private Abiku _currentAbiku;
    // [SerializeField] private List<Abiku> abikuses = new List<Abiku>();
    // private int currentAbikuIndex = -1;
    
    [Header("egungun")]
    [SerializeField] private Egungun egungun;
    
    [Header("stance")]
    public StateMachine StanceStateMachine;
    
    private List<AbikuStance> stances = new List<AbikuStance>();
    private int currentStateIndex = 0;
    
    [Header("GameCues")]
    public GameCue[] onHitCues;
    
    //Unity Events
    public event Action onAbilityModify;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (egungun == null)
        {
            Debug.LogError("NEW ERROR: egungun is null");
        }
        StanceStateMachine =  new StateMachine();

        foreach (var stance in trioDefinition.startingStances)
        {
            stances.Add(stance.CreateAbikuStanceState(this, StanceStateMachine));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Initialize()
    {
        if (trioDefinition == null)
        {
            Debug.LogError("TrioDefinition is null");
            return;
        }
    }
    
    public void MoveToCell(GridCell cell)
    {
        GetHoldingCell().EmptyCell();
        transform.position = cell.WorldPosition;
        SetHoldingCell(cell);
    }
    
    public void ChangeAbiku()
    {
        // currentAbikuIndex++;
        //
        // currentAbikuIndex = currentAbikuIndex % abikuses.Count;
        // _currentAbiku =  abikuses[currentAbikuIndex];
    }
    
    //interface
    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Highlight(CellHighlightState mode)
    {
        base.Highlight(mode);
        GetHoldingCell().AddHighlight(this,CellHighlightState.Targeted);
    }

    public override void UnHighlight()
    {
        base.UnHighlight();
        GetHoldingCell().RemoveHighlight(this);
    }
    //getter setter

    // public List<Abiku> GetAbikuses()
    // {
    //     return abikuses;
    // }
    // public Abiku GetCurrentAbiku()
    // {
    //     return _currentAbiku;
    // }
    //
    // public void SetCurrentAbiku(Abiku abiku)
    // {
    //     _currentAbiku = abiku;
    // }
    public TrioDefinition GetTrioDefinition()
    {
        return trioDefinition;
    }

    public void SetTrioDefinition(TrioDefinition newDefinition)
    {
        this.trioDefinition = newDefinition;
    }

    public Egungun GetEgungun()
    {
        return egungun;
    }
    

    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, ElementSO element)
    {
        Debug.Log("take damage abiku");
        foreach (var cue in onHitCues)
        {
            cue?.Execute(transform.position);
        }
        
        return new DamageInfo(source, this, abilityUsed, damage, element, null, false);
    }

    public void DEBUGPRINTALLSTANCESABILITIES()
    {
        
    }
}
