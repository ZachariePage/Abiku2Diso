using UnityEngine;

public class PlayerBattleStats : MonoBehaviour
{
    [SerializeField] private int _actionPerTurn = 0;
    [SerializeField] private int _currentActionPerTurn = 10;
    public static PlayerBattleStats Instance { get; private set; }
    
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
        _actionPerTurn = _currentActionPerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //getter setter
    public void ResetTurn()
    {
        _currentActionPerTurn = _actionPerTurn;
    }
    public int GetCurrentActionPerTurn()
    {
        return _currentActionPerTurn;
    }

    public void EncoreTriggered()
    {
        _currentActionPerTurn++;
        BattleLoop.Instance.EncoreTriggered();
    }

    public void DecrementAction()
    {
        _currentActionPerTurn--;
    }
}
