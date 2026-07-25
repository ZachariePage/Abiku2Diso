using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField] private MonoBehaviour selectedTargetDEBUGINSPECTORSHOWKEK;
    private readonly List<BattleEffect> activeEffects = new();
    private readonly List<ITargettable> actionHighlights = new();

    [SerializeField]
    private UsedActionTracker _tracker;
    private bool actionExecuting;
    
    private Queue<Enemy> enemyTurnQueue = new Queue<Enemy>();
    private List<AbikuTrio> abikuTrios = new List<AbikuTrio>();
    
    private bool encoreTriggered = false;
    
    //events
    public event Action onCombatStart;
    public event Action onPlayerTurnStart;

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
        
        onCombatStart?.Invoke();
        StartPlayerTurn();
    }
    public void OnTargetClicked(ITargettable target)
    {
        if(IsInputLocked()) return;
        //
        // if (actionExecuting)
        //     return;
        
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
            
            selectedTargetDEBUGINSPECTORSHOWKEK = selectedTarget as MonoBehaviour;
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

            selectedTargetDEBUGINSPECTORSHOWKEK = null;
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
        if (action is ICostGatedAction gated)
        {
            PlayerBattleStats.Instance.DecrementAction();
            
            if (encoreTriggered)
            {
                CurrentPhase = BattlePhase.Combat;
            }
            else
            {
                CurrentPhase = BattlePhase.TurnOver;
            }
        }
        
        action.PutOnColdown();
        encoreTriggered = false;
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
        
        if (action is ICostGatedAction gated)
        {
            
        }
        ClearPendingAction();
        pendingAction = action;
        if (action.TargetMode() == TargetMode.Instant)
        {
            action.AddTarget(selectedTarget);
            StartAction();
            ClearPendingAction();
            ClearSelectedTarget();
            return;
        }

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

    public void ClearSelectedTarget()
    {
        if(selectedTarget == null) return;
        selectedTarget.Deselect();
        selectedTarget = null;
    }

    //enemy turn
    public void PassTurn()
    {
        Debug.Log("passing turn");
        CurrentState = BattleState.AITurn;
        CurrentPhase  = BattlePhase.AITurn;
        ClearPendingAction();
        ClearSelectedTarget();
        
        _tracker.ResetTurn();
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("enemies turn are starting");
        foreach (var enemy in enemyTurnQueue)
        {
            TurnStartEvent turnEvent = new TurnStartEvent
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
    private void UpdateBattleEffects()
    {
        foreach (var effect in activeEffects)
        {
            effect.OnTurnStart();
        }

        activeEffects.RemoveAll(e => e.IsFinished());
    }

    public void AddEnemy(Enemy newEnemy)
    {
        if (newEnemy == null)
        {
            Debug.LogError("new enemy is null wtf");
            return;
        }
        enemyTurnQueue.Enqueue(newEnemy);
    }

    public void AddAbikuTrio(AbikuTrio newAbikuTrio)
    {
        if (newAbikuTrio == null)
        {
            Debug.LogError("new abiku is null wtf");
            return;
        }
        abikuTrios.Add(newAbikuTrio);
    }

    public void StartPlayerTurn()
    {
        Debug.Log("starting player turn");
        CurrentState = BattleState.Idle;
        CurrentPhase =  BattlePhase.Combat;
        PlayerBattleStats.Instance.ResetTurn();

        foreach (var abiku in abikuTrios)
        {
            abiku.OnTurnStart();
        }
        
        onPlayerTurnStart?.Invoke();
        
        _tracker.ResetTurn();
    }

    public void EncoreTriggered()
    {
        encoreTriggered = true;
        Debug.Log("encore triggered");
    }
    public void DEBUGSTARTCOMBAT()
    {
        StartCoroutine(StartCombat());
    }
}