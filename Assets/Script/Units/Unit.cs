using UnityEngine;

public class Unit : MonoBehaviour
{
    public IStateMachine StateMachine;

    public StanceStateScriptableObject ExempleState;

    [Header("State Configs")]
    [SerializeField] private StanceStateScriptableObject wanderConfig;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
