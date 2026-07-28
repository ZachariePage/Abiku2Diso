using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridActor : MonoBehaviour, ITargettable, IHoverable
{
    [Header("Parent grid actor settings")]
    private GridCell holdingCell;
    
    [SerializeField] protected HoverableUIData tooltipData;
    public event Action onSelection;
    public event Action onDeselection;
    
    private readonly List<BattleEffect> activeStartTurnEffects = new();
    private readonly List<BattleEffect> activeEndTurnEffects = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void Initialize()
    {
        
    }
    //getter setter
    public GridCell GetHoldingCell()
    {
        return holdingCell;
    }

    public void SetHoldingCell(GridCell cell)
    {
        holdingCell = cell;
        cell.SetActorOnCell(this);
    }

    public virtual void Select()
    {
        onSelection?.Invoke();
    }

    public virtual void Deselect()
    {
        onDeselection?.Invoke();
    }

    public virtual void Highlight(CellHighlightState mode)
    {
        
    }

    public virtual void UnHighlight()
    {
        
    }

    public virtual void AddHighlight(object source, CellHighlightState state)
    {
        
    }

    public virtual void RemoveHighlight(object source)
    {
        
    }

    public virtual TargettableTargetType GetTargetType()
    {
        return TargettableTargetType.monobehaviour;
    }

    public Vector2 GetWorldPosition()
    {
        return transform.position;
    }

    public override string ToString()
    {
        return $"Actor({gameObject}, at {GetWorldPosition()} of type {GetType()})";
    }

    public virtual HoverableUIData GetHoverData()
    {
        return tooltipData;
    }

    public HoverableUIType GetHoverType()
    {
        return HoverableUIType.UnitDescription;
    }

    //i could easily turn these two in one but ehhh
    protected IEnumerator ActivateStartOfTurnEffect()
    {
        foreach (var effect in activeStartTurnEffects)
        {
            yield return StartCoroutine(effect.OnTurnStart());
        }
        
        activeStartTurnEffects.RemoveAll(eff => eff.IsFinished());
    }
    protected IEnumerator ActivateEndOfTurnEffect()
    {
        foreach (var effect in activeEndTurnEffects)
        {
            yield return StartCoroutine(effect.OnTurnStart());
        }
        
        activeEndTurnEffects.RemoveAll(eff => eff.IsFinished());
    }
    
    public void AddBattleStartOfTurnEffect(BattleEffect effect)
    {
        activeStartTurnEffects.Add(effect);
    }
    public void AddBattleEndOfTurnEffect(BattleEffect effect)
    {
        activeEndTurnEffects.Add(effect);
    }
}
