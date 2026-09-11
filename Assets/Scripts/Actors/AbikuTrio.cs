using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AbikuTrio : GridActor,  IDamageable, IHoldElement, ISpellCaster
{
    [SerializeField] private TrioDefinition trioDefinition;
    
    [Header("egungun")]
    [SerializeField] private EgungunDefinition egungunDefinition;
    private Egungun egungun;
    
    [Header("stance")]
    public StateMachine<AbikuStance> StanceStateMachine;
    
    private Dictionary<BattleActionType, int> _bonusActions = new();
    private int _pendingSkillDiscount = 0;
    
    private List<AbikuStance> stances = new List<AbikuStance>();
    private int currentStateIndex = 0;
    private int StateIndexOnTurnStart = 0;
    private Element currentElement;
    
    private ActorEffectManager effectManager;
    
    private bool _stanceLocked;
    
    //stats
    private Health hpScript;
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
        if (egungunDefinition == null)
        {
            Debug.LogError("NEW ERROR: egungun is null");
        }
        StanceStateMachine =  new StateMachine<AbikuStance>();
        egungun = egungunDefinition.CreateEgungun(egungunDefinition, this);
        
        foreach (var stance in trioDefinition.startingStances)
        {
            stances.Add(stance.CreateAbikuStanceState(this, StanceStateMachine));
        }
        
        StanceStateMachine.Init(stances[0]);
        
        spriteRenderer =  GetComponent<SpriteRenderer>();

        tooltipData.name = trioDefinition.DisplayName;

        effectManager = GetComponent<ActorEffectManager>();

        Assert.IsNotNull(egungunDefinition, "no egungun wtf");

        hpScript = GetComponent<Health>();
        hpScript.Init(this, egungun.GetHP());
        defense = egungun.GetDefense();
        
        usedActionTracker =  new UsedActionTracker();
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
        //battleeffect
        yield return StartCoroutine(ActivateEndOfTurnEffect());
        onMyTurnEnd?.Invoke();
        //actoreffect
        effectManager.TriggerOnTurnEnd(this);
        yield return base.OnTurnEnd();
    }
    
    public override void MoveToCell(GridCell cell)
    {
        base.MoveToCell(cell);
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

    public void PutOnColdown()
    {
        
    }

    public void RefreshColdown()
    {
        Debug.Log("refresh coldown");
    }

    public bool IsOnColdown()
    {
        return !usedActionTracker.HasMoveLeft();
    }

    public bool CanThrowSpell()
    {
        Debug.Log("canThrowSpell");
        return false;
    }

    public bool IsCastingSpell()
    {
        Debug.Log("isCastingSpell");
        return false;
    }

    public void SetCastingSpell(bool value, CastingSpellColdownType type)
    {

    }

    public void OnAbilityThrown()
    {
        Debug.Log("onAbilityThrown");
    }   

    public UsedActionTracker GetCooldownTracker()
    {
        return usedActionTracker;
    }
    public void OnAbilityFinished(AbilityAftermathInfo abilityAftermathInfo)
    {
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
    public DamageInfo TakeDamage(GridActor source, IDamageSource damageSource, float damage, Element element)
    {
        Debug.Log("take damage abiku");
        Debug.LogWarning("not implemented");
        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(currentElement, element);
        if (encoreTriggered)
        {
            //TriggerEncore(this); no longer use i think
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
        hpScript.ModifyHp(-ctx.IncomingDamage);
        
        DamageInfo info = new DamageInfo(ctx.Source, this, damageSource, ctx.IncomingDamage, ctx.DamageElement, currentElement, false);
        onDamageTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        
        effectManager.TriggerOnDamageTaken(this,  info);
        return info;
    }
    public HealingInfo Heal(GridActor source, IHealingSource healingSource, float heal, Element element)
    {
        Debug.Log("healing abiku");
        hpScript.ModifyHp(heal);
        
        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(currentElement, element);
        if (encoreTriggered)
        {
            TriggerEncore(this);
        }
        
        HealingInfo info = new HealingInfo(source, this, healingSource, heal, element, currentElement, encoreTriggered);
        
        onHealTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        return info;
    }

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        effectManager.TriggerOutgoingDamage(ctx);
        return ctx;
    }

    public bool IsWounded()
    {
        return hpScript.IsWounded();
    }

    public Health GetHealth()
    {
        return hpScript;
    }

    public void TriggerEncore(GridActor usedActor)
    {
        onEncoreTriggered?.Invoke();
        PlayerBattleStats.Instance.EncoreTriggered(usedActor);
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

    public void Cheat_refreshcoldown()
    {
        foreach (AbikuStance abikuStance in stances)
        {
            abikuStance.Cheat_ResetTurn();
        }
    }
    
    public void Cheat_infiniteAbility()
    {
        foreach (AbikuStance abikuStance in stances)
        {
            abikuStance.Cheat_InfiniteAbilityUse();
        }
    }
}
