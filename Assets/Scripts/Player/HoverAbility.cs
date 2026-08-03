using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoverAbility : MonoBehaviour
{
    [Header("References")]
    [HideInInspector]
    public Camera cam;

    [SerializeField] private LayerMask clickableLayer;
    
    public IHoverable currentTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (currentTarget != null)
            {
                currentTarget = null;
                HoverTooltip.Instance.Hide();
            }
            return;
        }
        
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld, clickableLayer);

        if(hit == null)
        {
            if (currentTarget != null)
            {
                currentTarget = null;
            }
            HoverTooltip.Instance.Hide();
            return;
        }
        
        IHoverable target = hit.GetComponent<IHoverable>();

        if (target == currentTarget)
        {
            if (target != null)
            {
                HoverTooltip.Instance.SetPosition(Input.mousePosition);
            }
            return;
        }

        currentTarget = target;

        if (target == null)
        {
            HoverTooltip.Instance.Hide();
        }
        else
        {
            HoverTooltip.Instance.Show(target.GetHoverData(), Input.mousePosition, target.GetHoverType());
        }
    }
}
