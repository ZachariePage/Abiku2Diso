using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleStats : MonoBehaviour
{
    public static PlayerBattleStats Instance { get; private set; }
    private int momentum = 0;
    
    private HashSet<GridActor> actorsHitByEncore = new HashSet<GridActor>();
    
    private int _encoreCharges;
    [SerializeField] private int maxEncoreCharges = 3;
    
    public event Action onMomentumChanged;
    public event Action onEncoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EncoreTriggered(GridActor actor)
    {
        if(actorsHitByEncore.Contains(actor)) return;
        actorsHitByEncore.Add(actor);
        BattleLoop.Instance.EncoreTriggered();
        GrantEncoreCharge();
        IncreaseMomentum();
    }

    public void IncreaseMomentum()
    {
        momentum++;
        onMomentumChanged?.Invoke();
    }

    public int GetMomentum()
    {
        return momentum;
    }

    public void DecreaseMomentum(int manaCost)
    {
        momentum -= manaCost;
        if (momentum < 0)
        {
            momentum = 0;
        }
        onMomentumChanged?.Invoke();
    }

    public int GetEncoreCharges()
    {
        return _encoreCharges;
    }

    public bool HasRemainingEncore()
    {
        return _encoreCharges > 0;
    }

    public void GrantEncoreCharge()
    {
        _encoreCharges = Mathf.Min(_encoreCharges + 1, maxEncoreCharges);
        onEncoreChanged?.Invoke();
    }

    public bool ConsumeEncoreCharge()
    {
        if (_encoreCharges <= 0) return false;
        _encoreCharges--;
        onEncoreChanged?.Invoke();
        return true;
    }

    public void RefreshEncore()
    {
        _encoreCharges = 0;
        onEncoreChanged?.Invoke();
    }
}
