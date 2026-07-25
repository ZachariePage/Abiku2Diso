using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ClickAbility : MonoBehaviour
{
    [Header("References")]
    [HideInInspector]
    public Camera cam;
    [Header("Ray Settings")]
    private float maxDistance = 1000f;
    
    private Vector3 aimPoint; 
    private Vector3 fireDirection; 
    [SerializeField] private LayerMask clickableLayer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        InputListener.onRightClickEvent += OnRightClick;
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        
        if (!context.performed) return;
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld, clickableLayer);
 
        if (hit != null)
        {
            AbikuTrio hitAbiku = hit.GetComponent<AbikuTrio>();
 
            if (hitAbiku != null)
            {
                BattleLoop.Instance.OnTargetClicked(hitAbiku);
                return;
            }
        }
        
        if (hit != null)
        {
            GridActor actor = hit.GetComponent<GridActor>();
 
            if (actor != null)
            {
                BattleLoop.Instance.OnTargetClicked(actor);
                return;
            }
        }
        
        GridCell cell = TacticalGrid.Instance.GetCellFromWorldPosition(mouseWorld);
        if (cell != null)
        {
            BattleLoop.Instance.OnTargetClicked(cell);
            return;
        }
        
        BattleLoop.Instance.OnTargetClicked(null);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        aimPoint = ray.origin + ray.direction * maxDistance;
        fireDirection = ray.direction;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 0.02f);
    }
    //getter setter

}
