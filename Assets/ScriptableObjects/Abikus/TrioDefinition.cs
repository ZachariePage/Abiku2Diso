using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Trio")]
public class TrioDefinition : ScriptableObject
{
    public string DisplayName;
    public GameObject prefab;
    public HoverableUIData hoverData;
    [SerializeField] public List<AbikuStanceScriptableObject> startingStances = new List<AbikuStanceScriptableObject>();

    public int HP;
    public int moveRange;

    public int defence;
    
    public MovementDirections moveDirection;
    public DirectionType directionType;
}
