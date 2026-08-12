using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : GridActor,  IDamageable, IHoldElement, ISpellCaster
{
    [Header("state machie")]
    public StateMachine<EnemyStance> StateMachine;

    public EnemyStanceScriptableObject[] startingState;
    private List<EnemyStance> states = new List<EnemyStance>();
    private int currentStateIndex = 0;

    public ActorEffectManager effectManager;

    private Health hpScript;
    private Element currentElement;
    
    private UsedActionTracker _coldownTracker;
    
    private bool currentlyCasting = false;

    public int turnBeforeExecutingAction = -1;
    
    [SerializeField] private List<AbilityAction> abilityActions =  new List<AbilityAction>();
    
    //turn loop
    private bool _turnFinishedFlag = false;
    //evengts
    public event Action<DamageInfo> onDamageTaken;
    public event Action<HealingInfo> onHealTaken;
    public event Action<DamageInfo> onStartTurn;

    public event Action onStanceChange;
    
    //event cues
    public event Action onDamageTakenCues;
    public event Action onHealTakenCues;
    public event Action onStartTurnCues;
    
    //debug
    public GameCue DEBUGCASTINGTEXTCUE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        StateMachine = new StateMachine<EnemyStance>();
        if (startingState.Length == 0)
        {
            Debug.LogError("THERE ARE NO STARTING STATE REEEEEEEEEEEEE");
        }
        foreach (var stateSO in startingState)
        {
            EnemyStance state = stateSO.CreateEnemyState(this, StateMachine);
            states.Add(state);
        }

        effectManager = GetComponent<ActorEffectManager>();
        
        hpScript = GetComponent<Health>();
        //TODO give enemy real hp
        hpScript.Init(this, 100);
        
        StateMachine.Init(states[0]);
        onStanceChange?.Invoke();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Init()
    {
        
    }
    
    public IEnumerator StartOfCombat()
    {
        StateMachine.ChangeState(states[0]);
        
        yield return null;
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
        StartCoroutine(ActivateStartOfTurnEffect());
        
        //here probably check if still alive after effect
        _turnFinishedFlag = false;
        
        StateMachine.CurrentState.StartTurn();
        
        onStartTurnCues?.Invoke();
        
        yield return new WaitForSeconds(2f);
        
        yield return new WaitUntil(() => _turnFinishedFlag);
        
        StartCoroutine(ActivateEndOfTurnEffect());
    }

    public void EndTurn()
    {
        _turnFinishedFlag = true;
    }

    public void ExecuteAction(AbilityAction action, Action onActionFinished)
    {
        ActionTakenEvent actionTakenEvent = new ActionTakenEvent
        {
            Action = action,
            Actor = action.GetActorOwner(),
            Targets = action.GetTargets().ToList(),
        };
        StartCoroutine(action.Execute(onActionFinished));
    }

    public void ChangeStateThroughIncrementation()
    {
        currentStateIndex = (currentStateIndex + 1) % states.Count;
        StateMachine.ChangeState(states[currentStateIndex]);
        onStanceChange?.Invoke();
    }


    //for now to see if encore is triggered will be here but in the future ill make a damage computation script
    public DamageInfo TakeDamage(GridActor source, IDamageSource damageSource, float damage, Element element)
    {
        Debug.LogWarning("damage modify effect not implemented");
        hpScript.ModifyHp(-damage);

        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(element, currentElement);
        
        DamageInfo info = new DamageInfo(source, this, damageSource, damage, element, currentElement, encoreTriggered);
        
        onDamageTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        
        effectManager.TriggerOnDamageTaken(this,  info);
        return info;
    }

    public HealingInfo Heal(GridActor source, IHealingSource healingSource, float heal, Element element)
    {
        hpScript.ModifyHp(heal);

        bool encoreTriggered = ElementSystem.Instance.IsEffectiveAgainst(element, currentElement);
        
        HealingInfo info = new HealingInfo(source, this, healingSource, heal, element, currentElement, encoreTriggered);
        
        onHealTaken?.Invoke(info);
        onDamageTakenCues?.Invoke();
        return info;
    }

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        Debug.LogWarning("not implemented");
        return ctx;
    }

    public bool IsWounded()
    {
        throw new NotImplementedException();
    }

    public Health GetHealth()
    {
        return hpScript;
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
    
    //debugs

    public void SpawnText(string text)
    {
        GameObject obj = DEBUGCASTINGTEXTCUE?.Execute(transform.position);
        TextMeshProUGUI textMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;
    }

    public Element GetElement()
    {
        return  currentElement;
    }

    public void PutAbilityOnColdown(BattleAction action)
    {
        
    }

    public void PutOnColdown()
    {
        return;
    }

    public void RefreshColdown()
    {
        return;
    }

    public bool IsOnColdown()
    {
        return false;
    }

    public bool CanThrowSpell()
    {
        return true;
    }

    public bool IsCastingSpell()
    {
        return currentlyCasting;
    }

    public void SetCastingSpell(bool value, CastingSpellColdownType type)
    {
        switch (type)
        {
            case CastingSpellColdownType.enemy:
                currentlyCasting = value;
                break;
            case CastingSpellColdownType.player:
                break;
            case CastingSpellColdownType.both:
                currentlyCasting = value;
                break;
        }
    }

    public void OnAbilityThrown()
    {
        
    }

    public void OnAbilityFinished(AbilityAftermathInfo abilityAftermathInfo)
    {
        ChangeStateThroughIncrementation();
    }

    public UsedActionTracker GetCooldownTracker()
    {
        return _coldownTracker;
    }

    public override Team GetMyTeam()
    {
        return Team.enemies;
    }
}
