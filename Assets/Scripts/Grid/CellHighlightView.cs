using System;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlightView : MonoBehaviour
{
    private SpriteRenderer targetRenderer;
    [SerializeField] private Color hoveredColor = Color.green;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color moveColor = Color.green;
    [SerializeField] private Color attackColor = Color.green;
    [SerializeField] private Color targetColor = Color.green;
    [SerializeField] private Color reachable = Color.green;
    
    private void Start()
    {
        
    }

    public void Init()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void SetState(GridCell cell)
    {
        IReadOnlyCollection<CellHighlightState> active = cell.ActiveHighlights;

        if (active.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Color sum = Color.clear;
        int count = 0;

        foreach (CellHighlightState state in active)
        {
            if (state == CellHighlightState.None)
                continue;

            sum += ColorFor(state);
            count++;
        }

        targetRenderer.color = count > 0 ? sum / count : Color.clear;
    }

    private Color ColorFor(CellHighlightState state)
    {
        switch (state)
        {
            case CellHighlightState.Hovered: return hoveredColor;
            case CellHighlightState.Selected: return selectedColor;
            case CellHighlightState.MoveRange: return moveColor;
            case CellHighlightState.AttackRange: return attackColor;
            case CellHighlightState.Targeted: return targetColor;
            case CellHighlightState.Reachable: return reachable;
            default: return Color.clear;
        }
    }
    
}