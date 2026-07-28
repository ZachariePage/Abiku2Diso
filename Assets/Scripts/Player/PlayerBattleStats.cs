using System;
using UnityEngine;

public class PlayerBattleStats : MonoBehaviour
{
    public static PlayerBattleStats Instance { get; private set; }
    private int momentum = 0;
    
    public event Action onMomentumChanged;

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

    public void EncoreTriggered()
    {
        BattleLoop.Instance.EncoreTriggered();
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

}
