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
    
    [Header("Actions")]
    [SerializeField] private List<AbilityAction> trioActions;
    private MoveAbikuAction moveAction;
    private ChangeAbikuAction changeAction;
    
    [Header("GameCues")]
    public GameCue[] onHitCues;
    
    //Unity Events
    public event Action onAbilityModify;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = new MoveAbikuAction(this);
        changeAction = new ChangeAbikuAction(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Initialize()
    {
        foreach (var action in trioDefinition.startingActions)
        {
            AddAbility(action.CreateAction(this));
        }
        // if (trioDefinition == null)
        // {
        //     Debug.LogError("TrioDefinition is null");
        //     return;
        // }
        //
        // if (abikuses.Count == 0)
        // {
        //     foreach (Abiku abiku in trioDefinition.startingAbiku)
        //     {
        //         AbikuDefinition def = abiku.GetAbikuDefinition();
        //         Abiku newAbiku = new Abiku(def, def.baseHealth, def.baseDamage, this);
        //         newAbiku.Initialize();
        //         abikuses.Add(newAbiku);
        //     }
        // }
        //
        // _currentAbiku =  abikuses[0];
        // currentAbikuIndex = 0;
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
    
    public void AddAbility(AbilityAction abilityAction)
    {
        trioActions.Add(abilityAction);
        onAbilityModify?.Invoke();
    }

    public void RemoveAbility(AbilityAction abilityAction)
    {
        trioActions.Remove(abilityAction);
        onAbilityModify?.Invoke();
    }
    
    public MoveAbikuAction GetMoveAction()
    {
        return moveAction;
    }

    public ChangeAbikuAction GetChangeAction()
    {
        return changeAction;
    }

    public IEnumerable<AbilityAction> GetAbilityActions()
    {
        return trioActions;
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
}
