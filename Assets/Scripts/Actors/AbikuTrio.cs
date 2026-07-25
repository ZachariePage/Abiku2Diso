using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbikuTrio : GridActor,  IDamageable, IHoldElement
{
    [SerializeField] private TrioDefinition trioDefinition;
    // [SerializeField] private Abiku _currentAbiku;
    // [SerializeField] private List<Abiku> abikuses = new List<Abiku>();
    // private int currentAbikuIndex = -1;
    
    [Header("egungun")]
    [SerializeField] private Egungun egungun;
    
    [Header("stance")]
    public StateMachine<AbikuStance> StanceStateMachine;
    
    private List<AbikuStance> stances = new List<AbikuStance>();
    private int currentStateIndex = 0;
    private int StateIndexOnTurnStart = 0;
    
    //Unity Events
    public event Action onAbilityModify;
    public event Action<DamageInfo> onDamageTaken;
    
    //event cues
    public event Action onDamageTakenCues;
    public event Action onChangeStance;

    public event Action onMyTurnStart;
    
    private UsedActionTracker usedActionTracker;
    
    //renderer. Properly should put this in another script idk
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (egungun == null)
        {
            Debug.LogError("NEW ERROR: egungun is null");
        }
        StanceStateMachine =  new StateMachine<AbikuStance>();

        foreach (var stance in trioDefinition.startingStances)
        {
            stances.Add(stance.CreateAbikuStanceState(this, StanceStateMachine));
        }
        
        StanceStateMachine.Init(stances[0]);
        
        spriteRenderer =  GetComponent<SpriteRenderer>();
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

    public void OnTurnStart()
    {
        StateIndexOnTurnStart = currentStateIndex;
        onMyTurnStart?.Invoke();
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

    public void ChangeStance()
    {
        currentStateIndex++;
        currentStateIndex = currentStateIndex % stances.Count;
        StanceStateMachine.ChangeState(stances[currentStateIndex]);
        onChangeStance?.Invoke();
    }
    
    //interface
    public override void Select()
    {
        base.Select();
        TurnStartEvent turnEvent = new TurnStartEvent
        {
            Actor = this
        };
        BattleStats.Instance.Broadcast(turnEvent);
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Highlight(CellHighlightState mode)
    {
        base.Highlight(mode);
        Debug.Log("MEOWINGTONG");
    }

    public override void UnHighlight()
    {
        base.UnHighlight();
        
    }

    // for now this but later gotta add the source just like in the gridcell. But gridcell need a refactor cuz its disgusting
    public override void AddHighlight(object source, CellHighlightState state)
    {
        base.AddHighlight(source, state);
        GetHoldingCell().AddHighlight(this,CellHighlightState.Targeted);
        spriteRenderer.material.SetFloat("_OutlineThickness", 60f);
    }

    public override void RemoveHighlight(object source)
    {
        base.RemoveHighlight(source);
        GetHoldingCell().RemoveHighlight(this);
        spriteRenderer.material.SetFloat("_OutlineThickness", 0f);
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
    

    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, Element element)
    {
        Debug.Log("take damage abiku");
        DamageInfo info = new DamageInfo(source, this, abilityUsed, damage, element, Element.None, false);
        onDamageTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        return info;
    }

    public void DEBUGPRINTALLSTANCESABILITIES()
    {
        foreach (var stance in stances)
        {
            foreach (var ability in stance.GetAbilities())
            {
                Debug.Log(ability);
            }
        }
    }

    public Element GetElement()
    {
        return Element.None;
    }

    public bool IsOnLastStance()
    {
        int nextStance = (currentStateIndex + 1) % stances.Count;
        return nextStance == StateIndexOnTurnStart;
    }
}
