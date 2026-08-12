using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public enum BattleState
{
    PreparationPhase,
    Idle,
    PlayerSelectingTarget,   
    ExecutingAction,  
    TurnOver,
    AITurn,
    BattleOver
}

[Flags]
public enum BattlePhase
{
    None = 0,
    Preparation = 1 << 0,
    Combat = 1 << 1,
    AITurn = 1 << 2,
    TurnOver = 1 << 3,
    All = ~0
}

public class BattleLoop : MonoBehaviour
{
    public static BattleLoop Instance { get; private set; }

    public BattleState CurrentState { get; private set; }
    public BattlePhase CurrentPhase { get; private set; }
    
    private BattleAction pendingAction;
    private HashSet<ITargettable> validTargets;
    
    [SerializeField] private ITargettable selectedTarget;
    private List<ITargettable> selectedTargets = new();
    private AbikuTrio currentlySelectedAbikuTrio;
    
    private List<BattleEffect> activeEffects = new();
    private List<ITargettable> actionHighlights = new();

    [SerializeField]
    private UsedActionTracker _tracker;
    private bool actionExecuting;
    
    private Queue<Enemy> enemyTurnQueue = new Queue<Enemy>();
    private List<AbikuTrio> abikuTrios = new List<AbikuTrio>();
    
    private bool encoreTriggered = false;
    
    //events
    public event Action OnCombatStart;
    public event Action OnPlayerTurnStart;
    public event Action<IReadOnlyList<IActionOption>> OnChoicePrompt;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _tracker = new UsedActionTracker();
        
        CurrentState = BattleState.PreparationPhase;
        CurrentPhase = BattlePhase.Preparation;
    }

    public IEnumerator StartCombat()
    {
        foreach (var enemy in enemyTurnQueue)
        {
            yield return StartCoroutine(enemy.StartOfCombat());
        }
        
        OnCombatStart?.Invoke();
        StartPlayerTurn();
    }
    public void OnTargetClicked(ITargettable target)
    {
        if(IsInputLocked()) return;
        
        if (target == null)
        {
            CurrentState = BattleState.Idle;
            ClearPendingAction();
            ClearSelectedTarget();
            return;
        }
        
        if (pendingAction == null)
        {
            if(selectedTarget != null) selectedTarget.Deselect();
            
            CurrentState = BattleState.PlayerSelectingTarget;
            selectedTarget = target;
            selectedTarget.Select();
            
            return;
        }
        
        if (!validTargets.Contains(target))
        {
            ClearSelectedTarget();
            ClearPendingAction();
            return;
        }

        pendingAction.AddTarget(target);

        if (pendingAction.IsReady())
        {
            StartAction();
            
            ClearPendingAction();
            ClearSelectedTarget();
        }
    }
    
    private bool IsInputLocked()
    {
        return CurrentState == BattleState.BattleOver || CurrentState == BattleState.AITurn || actionExecuting;
    }

    private void StartAction()
    {
        actionExecuting = true;
        BattleAction action = pendingAction;

        ActionTakenEvent actionTakenEvent = new ActionTakenEvent
        {
            Action = action,
            Actor = action.GetActorOwner(),
            Targets = action.GetTargets().ToList(),
        };
        BattleStats.Instance.Broadcast(actionTakenEvent);
        StartCoroutine(action.Execute(() => OnActionFinished(action)));
        
        CurrentState = BattleState.ExecutingAction;
    }

    private void OnActionFinished(BattleAction action)
    {
        if (action is not ICostGatedAction gated)
        {
            actionExecuting = false;
            ClearPendingAction();
            ClearSelectedTarget();
            return;
        }
        ISpellCaster caster = gated.Caster();
        
        if (encoreTriggered)
        {
            caster.GetCooldownTracker().SetEncoreTriggered(true);
            encoreTriggered = false; 
        }
        
        bool turnOver = caster.GetCooldownTracker().RegisterActionAndCheckTurnOver(gated.GetActionType());

        if (turnOver)
        {
            caster.PutOnColdown();
        }

        bool allTurnOver = true;

        foreach (AbikuTrio trio in abikuTrios)
        {
            ISpellCaster stance = trio.StanceStateMachine.CurrentState;
            if (!stance.IsOnColdown())
            {
                allTurnOver = false;
                break;
            }
        }

        if (allTurnOver)
        {
            CurrentPhase = BattlePhase.TurnOver;
        }
        else
        {
            CurrentPhase = BattlePhase.Combat;
        }
        
        PlayerBattleStats.Instance.DecreaseMomentum(action.ManaCost());
        actionExecuting = false;
        ClearPendingAction();
        ClearSelectedTarget();
    }
    
    public void SetPendingAction(BattleAction action)
    {
        if (!action.CanBeUsedNow(CurrentPhase))
        {
            Debug.Log($"{action} can't be used during {CurrentPhase}");
            ClearPendingAction();
            ClearSelectedTarget();
            return;
        }

        if (currentlySelectedAbikuTrio != action.GetActorOwner() && currentlySelectedAbikuTrio != null)
        {
            Debug.Log($"{action.GetActorOwner()} isn't {currentlySelectedAbikuTrio}");
            ClearPendingAction();
            ClearSelectedTarget();
            return;
        }
        if (action is ICostGatedAction gated)
        {
            
        }
        ClearPendingAction();
        pendingAction = action;
        switch (action.TargetMode())
        {
            case TargetMode.Instant:
                action.AddTarget(selectedTarget);
                StartAction();
                ClearPendingAction();
                ClearSelectedTarget();
                return;

            case TargetMode.Choice:
                selectedTarget.Deselect();
                PromptChoice(action);
                return;

            case TargetMode.Single:
            case TargetMode.Multiple:
                BeginTargeting(action);
                return;
        }
    }
    
    //inprogress
    private bool PromptChoice(BattleAction action)
    {
        if (action is IChoiceGatedAction choiceAction)
        {
            OnChoicePrompt?.Invoke(choiceAction.GetOptions());
            return true;
        }
        return false;
    }
    
    public void SelectPendingOption(IActionOption option)
    {
        if (pendingAction is not IChoiceGatedAction choiceAction) return;
        if (!choiceAction.SelectOption(option)) return;

        if (choiceAction.StartActionImmediately())
        {
            StartAction();
            ClearPendingAction();
            ClearSelectedTarget();
        }
        else
        {
            BeginTargeting(pendingAction);
        }
    }
    
    private void BeginTargeting(BattleAction action)
    {
        ClearSelectedTarget();
        RefreshTargetHighlights(action);
    }
    private void RefreshTargetHighlights(BattleAction action)
    {
        foreach (var targetToClear in actionHighlights)
        {
            targetToClear.RemoveHighlight(this);
        }
        actionHighlights.Clear();
        
        validTargets = new HashSet<ITargettable>(action.GetValidTargets());

        foreach(var target in validTargets)
        {
            target.AddHighlight(this, action.GetHighlightState());
            actionHighlights.Add(target);
        }
        
        List<GridCell> reachableCell = new List<GridCell>(action.GetReachableCells());
        
        foreach (var cell in reachableCell)
        {
            if (cell is ITargettable targettable)
            {
                cell.AddHighlight(this, CellHighlightState.Reachable);
                actionHighlights.Add(cell);
            }
        }
    }
    
    //inprogress
    private void ClearActionHighlights()
    {
        foreach(var target in actionHighlights)
        {
            target.RemoveHighlight(this);
        }

        actionHighlights.Clear();
    }
    
    public void ClearPendingAction()
    {
        pendingAction = null;
        ClearActionHighlights();
    }

    public void ClearSelectedAbikuTrio()
    {
        currentlySelectedAbikuTrio =  null;
    }

    public void ClearSelectedTarget()
    {
        if(selectedTarget == null) return;
        selectedTarget.Deselect();
        selectedTarget = null;
    }

    //enemy turn
    public void PassTurn()
    {
        if ((CurrentPhase & (BattlePhase.Combat | BattlePhase.TurnOver)) == 0)
        {
            Debug.Log($"Can't pass turn in {CurrentPhase}");
            return;
        }
        
        foreach (AbikuTrio abiku in abikuTrios)
        {
            StartCoroutine(abiku.OnTurnEnd());
        }
        
        CurrentState = BattleState.AITurn;
        CurrentPhase  = BattlePhase.AITurn;
        
        ClearSelectedAbikuTrio();
        ClearPendingAction();
        ClearSelectedTarget();
        
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        foreach (var enemy in enemyTurnQueue)
        {
            GridActorTurnStartEvent turnEvent = new GridActorTurnStartEvent
            {
                Actor = enemy
            };
            BattleStats.Instance.Broadcast(turnEvent);
            yield return StartCoroutine(enemy.TakeTurn());
        }

        EndTurnOfEnemies();
    }

    private void EndTurnOfEnemies()
    {
        StartCoroutine(ActivateEndOfTurnEffect());
        
        StartPlayerTurn();
    }

    public IEnumerator ActivateEndOfTurnEffect()
    {
        foreach (var effect in activeEffects)
        {
            yield return StartCoroutine(effect.OnTurnStart());
        }
        
        activeEffects.RemoveAll(eff => eff.IsFinished());
    }
    //getter setter add
    public void AddBattleEffect(BattleEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void AddEnemy(Enemy newEnemy)
    {
        Assert.IsNotNull(newEnemy, "new enemy is null wtf");
        enemyTurnQueue.Enqueue(newEnemy);
    }

    public void AddAbikuTrio(AbikuTrio newAbikuTrio)
    {
        if (newAbikuTrio == null)
        {
            throw new ArgumentNullException(nameof(newAbikuTrio), "new enemy is null wtf");
        }
        
        abikuTrios.Add(newAbikuTrio);
    }

    public ITargettable GetSelectedTarget()
    {
        return selectedTarget;
    }

    public void StartPlayerTurn()
    {
        TurnStartEvent turnEvent = new TurnStartEvent
        {
            team = Team.allies
        };
        BattleStats.Instance.Broadcast(turnEvent);
        
        CurrentState = BattleState.Idle;
        CurrentPhase =  BattlePhase.Combat;

        foreach (var abiku in abikuTrios)
        {
            StartCoroutine(abiku.OnTurnStart());
        }
        
        OnPlayerTurnStart?.Invoke();
    }

    public void EncoreTriggered()
    {
        encoreTriggered = true;
    }

    public bool IsActionPending()
    {
        return pendingAction != null;
    }

    public IReadOnlyBattleAction GetReadOnlyPendingAction()
    {
        return pendingAction;
    }
    public void DEBUGSTARTCOMBAT()
    {
        StartCoroutine(StartCombat());
    }

    public void Cheat_SetbattlePhase(BattlePhase battlePhase)
    {
        CurrentPhase = battlePhase;
    }
}