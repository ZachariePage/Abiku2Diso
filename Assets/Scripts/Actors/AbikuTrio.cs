using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AbikuTrio : GridActor,  IDamageable, IHoldElement
{
    [SerializeField] private TrioDefinition trioDefinition;
    
    [Header("egungun")]
    [SerializeField] private Egungun egungun;
    
    [Header("stance")]
    public StateMachine<AbikuStance> StanceStateMachine;
    
    private readonly Dictionary<BattleActionType, int> _bonusActions = new();
    private int _pendingSkillDiscount = 0;
    
    private List<AbikuStance> stances = new List<AbikuStance>();
    private int currentStateIndex = 0;
    private int StateIndexOnTurnStart = 0;
    private Element currentElement;
    
    private ActorEffectManager effectManager;
    
    private bool _stanceLocked;
    
    //stats
    private float health;
    private int defense;
    
    //Unity Events
    public event Action onAbilityModify;
    public event Action onAbilityFinished;
    public event Action<DamageInfo> onDamageTaken;
    public event Action<HealingInfo> onHealTaken;
    
    //event cues
    public event Action onDamageTakenCues;
    public event Action onHealTakenCues;
    public event Action onChangeStance;

    public event Action onMyTurnStart;
    public event Action onMyTurnEnd;
    
    public event Action onEncoreTriggered;
    
    private UsedActionTracker usedActionTracker;
    
    //renderer. Properly should put this in another script idk
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
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

        tooltipData.name = trioDefinition.DisplayName;

        effectManager = GetComponent<ActorEffectManager>();

        Assert.IsNotNull(egungun, "no egungun wtf");
        health = egungun.GetHP();
        defense = egungun.GetDefense();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
    public override void Initialize()
    {
        if (trioDefinition == null)
        {
            Debug.LogError("TrioDefinition is null");
            return;
        }
    }

    public override IEnumerator OnTurnStart()
    {
        effectManager.TriggerOnTurnStart(this);
        StateIndexOnTurnStart = currentStateIndex;
        onMyTurnStart?.Invoke();
        yield return base.OnTurnStart();
    }
    
    public override IEnumerator OnTurnEnd()
    {
        ClearBonusActions();
        yield return StartCoroutine(ActivateEndOfTurnEffect());
        onMyTurnEnd?.Invoke();
        effectManager.TriggerOnTurnEnd(this);
        yield return base.OnTurnEnd();
    }
    
    public override void MoveToCell(GridCell cell)
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
        GridActorTurnStartEvent turnEvent = new GridActorTurnStartEvent
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
    }

    public override void UnHighlight()
    {
        base.UnHighlight();
        
    }
    
    public void OnAbilityFinished(AbilityAftermathInfo abilityAftermathInfo)
    {
        Debug.Log("one");
        effectManager.TriggerOnAbilityFinished(this, abilityAftermathInfo);
    }

    // for now this but later gotta add the source just like in the gridcell. But gridcell need a refactor cuz its disgusting
    public override void AddHighlight(object source, CellHighlightState state)
    {
        base.AddHighlight(source, state);
        spriteRenderer.material.SetFloat("_OutlineThickness", 60f);
    }

    public override void RemoveHighlight(object source)
    {
        base.RemoveHighlight(source);
        spriteRenderer.material.SetFloat("_OutlineThickness", 0f);
    }
    public TrioDefinition GetTrioDefinition()
    {
        return trioDefinition;
    }
    
    public override HoverableUIData GetHoverData()
    {
        return trioDefinition.hoverData;
    }

    public void SetTrioDefinition(TrioDefinition newDefinition)
    {
        this.trioDefinition = newDefinition;
    }

    public Egungun GetEgungun()
    {
        return egungun;
    }
    

    //for now to see if encore is triggered will be here but in the future ill make a damage computation script
    public DamageInfo TakeDamage(GridActor source, AbilityAction abilityUsed, float damage, Element element)
    {
        Debug.Log("take damage abiku");
        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(currentElement, element);
        if (encoreTriggered)
        {
            TriggerEncore();
        }
        
        DamageMitigationContext ctx = new DamageMitigationContext
        {
            Self = this,
            Source = source,
            DamageElement = element,
            IncomingDamage = damage,
            Defense = defense 
        };
        
        effectManager.TriggerDamageMitigation(ctx);
        
        DamageInfo info = new DamageInfo(source, this, abilityUsed, damage, element, currentElement, false);
        onDamageTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        return info;
    }
    public HealingInfo Heal(GridActor source, AbilityAction abilityUsed, float heal, Element element)
    {
        Debug.Log("healing abiku");
        health += heal;

        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(currentElement, element);
        if (encoreTriggered)
        {
            TriggerEncore();
        }
        
        HealingInfo info = new HealingInfo(source, this, abilityUsed, heal, element, currentElement, encoreTriggered);
        
        onHealTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        return info;
    }

    public void BuffDefense(int value)
    {
        
    }

    public int ModifyIncomingDamage(int value)
    {
        Debug.LogWarning("not implemented");
        return value;
    }

    public void TriggerEncore()
    {
        onEncoreTriggered?.Invoke();
        PlayerBattleStats.Instance.EncoreTriggered();
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
        return currentElement;
    }

    public void SetElement(Element element)
    {
        currentElement = element;
    }

    public bool IsOnLastStance()
    {
        int nextStance = (currentStateIndex + 1) % stances.Count;
        return nextStance == StateIndexOnTurnStart;
    }

    public override Team GetMyTeam()
    {
        return Team.allies;
    }
    
    public void GrantBonusAction(BattleActionType type)
    {
        _bonusActions.TryGetValue(type, out int count);
        _bonusActions[type] = count + 1;
    }

    public bool HasBonusAction(BattleActionType type)
    {
        return _bonusActions.TryGetValue(type, out int count) && count > 0;
    }

    public bool HasAnyBonusAction()
    {
        return _bonusActions.Values.Any(c => c > 0);
    }

    public void ConsumeBonusAction(BattleActionType type)
    {
        if (_bonusActions.TryGetValue(type, out int count) && count > 0)
        {
            _bonusActions[type] = count - 1;
        }
    }

    public void ClearBonusActions() 
    {
        _bonusActions.Clear();
    }
    
    public void SetStanceLocked(bool locked)
    {
        _stanceLocked = locked;
    }

    public bool IsStanceLocked()
    {
        return _stanceLocked;
    }

    public void GrantNextSkillDiscount(int amount)
    {
        _pendingSkillDiscount += amount;
    }
    
    public int ConsumeSkillDiscount()
    {
        int discount = _pendingSkillDiscount;
        _pendingSkillDiscount = 0;
        return discount;
    }

    public int GetSkillDiscount()
    {
        return _pendingSkillDiscount;
    }
}
