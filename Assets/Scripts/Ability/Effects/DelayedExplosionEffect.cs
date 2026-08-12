using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DelayedExplosionEffect : DamagingBattleEffect
{
    private List<GridCell> _cells =  new List<GridCell>();
    private GridCell _targetCell;
    private int _turnsRemaining;
    private int _damage;
    private int _range;
    private MovementDirections _direction;


    public DelayedExplosionEffect(ElementSO element, GridCell targetCell, int turnsRemaining, int damage, int range, MovementDirections direction) : base(element)
    {
        _targetCell = targetCell;
        _turnsRemaining = turnsRemaining;
        _damage = damage;
        _range = range;
        _direction = direction;
        _element = element;
        
        BattleLoop.Instance.AddBattleEffect(this);
        ChooseCells();
    }

    public event Action onEventCompletion;

    

    public override IEnumerator OnTurnStart()
    {
        _turnsRemaining--;
        if (_turnsRemaining > 0) yield break;
        
        DealDamageToTargets(_cells, _damage);

        yield return new WaitForSeconds(1f); 
        
        onEventCompletion?.Invoke();
        Cleanup();
        isFinished = true;
        
        yield return null;
    }

    public override bool IsFinished()
    {
        return isFinished;
    }

    private void ChooseCells()
    {
        List<GridCell> targets = GridPathfinder.GetReachableCells(_targetCell, _range, _direction, true);
        foreach (var target in targets)
        {
            _cells.Add(target);
            target.AddHighlight(this,CellHighlightState.Targeted);
        }
    }

    private void Cleanup()
    {
        foreach (var cell in _cells)
        {
            cell.RemoveHighlight(this);
        }
    }
    
    
}
