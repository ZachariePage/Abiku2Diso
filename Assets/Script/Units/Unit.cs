using UnityEngine;

public class Unit : MonoBehaviour
{
    public StateMachine StateMachine;

    public StanceStateScriptableObject ExempleState;

    [Header("State Configs")]
    [SerializeField] private StanceStateScriptableObject wanderConfig;
    void Start()
    {
        StateMachine =  new StateMachine();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
