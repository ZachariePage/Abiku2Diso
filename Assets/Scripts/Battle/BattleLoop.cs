using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Idle,
    PlayerSelectingTarget,   
    ExecutingAction,         
    AITurn,
    BattleOver
}

public class BattleLoop : MonoBehaviour
{
    public static BattleLoop Instance { get; private set; }

    public BattleState CurrentState { get; private set; }
    
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
    
    Queue<Enemy> enemyTurnQueue = new Queue<Enemy>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _tracker = new UsedActionTracker();
    }


    public void OnTargetClicked(ITargettable target)
    {
        if(CurrentState == BattleState.AITurn || CurrentState == BattleState.BattleOver) return;
        
        if (actionExecuting)
            return;
        
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

    private void StartAction()
    {
        actionExecuting = true;
        BattleAction action = pendingAction;
        
        StartCoroutine(action.Execute(() => OnActionFinished(action)));
        
        CurrentState = BattleState.ExecutingAction;
    }

    private void OnActionFinished(BattleAction action)
    {
        if (action is ICostGatedAction gated)
        {
            _tracker.MarkUsed(gated.Performer(), gated.GetActionType());
            PlayerBattleStats.Instance.DecrementAction();
            Debug.Log($"{gated.Performer()} finished using type {gated.GetActionType()}");
        }
        actionExecuting = false;

        ClearPendingAction();
        ClearSelectedTarget();

        CurrentState = BattleState.Idle;
        
        if (PlayerBattleStats.Instance.GetCurrentActionPerTurn() <= 0)
        {
            Debug.Log("no more actions");
            PassTurn();
        }
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
    
    public void SetPendingAction(BattleAction action)
    {
        if (action is ICostGatedAction gated)
        {
            if (_tracker.HasUsed(gated.Performer(), gated.GetActionType()))
            {
                Debug.Log($"{gated.Performer()} has used type {gated.GetActionType()} already");
                ClearPendingAction();
                ClearSelectedTarget();
                return;
            }
            if (PlayerBattleStats.Instance.GetCurrentActionPerTurn() <= gated.ManaCost())
            {
                Debug.Log($"no more mana");
                ClearPendingAction();
                ClearSelectedTarget();
                return;
            }
        }
        
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

    //enemy turn
    public void PassTurn()
    {
        Debug.Log("passing turn");
        CurrentState = BattleState.AITurn;
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

    public void StartPlayerTurn()
    {
        Debug.Log("starting player turn");
        CurrentState = BattleState.Idle;
        PlayerBattleStats.Instance.ResetTurn();
        
        _tracker.ResetTurn();
    }
}