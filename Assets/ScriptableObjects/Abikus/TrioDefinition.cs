using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Trio")]
public class TrioDefinition : ScriptableObject
{
    public GameObject prefab;
    [SerializeField] public List<AbikuStanceScriptableObject> startingStances = new List<AbikuStanceScriptableObject>();

    public int HP;
    public int moveRange;

    public int defence;
    
    public MovementDirections moveDirection;
}
