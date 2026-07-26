using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleUI : MonoBehaviour
{
    [SerializeField] private Image stanceIcon;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemy.onStanceChange += StanceIcon;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void StanceIcon()
    {
        stanceIcon.sprite = enemy.StateMachine.CurrentState.GetStanceConfig().icon;
    }
}
