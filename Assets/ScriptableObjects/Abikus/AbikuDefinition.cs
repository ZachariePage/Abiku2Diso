using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Abiku")]
public class AbikuDefinition : ScriptableObject
{
    public GameObject prefab;
    public float baseHealth;
    public float baseDamage;
    public AbilityTemplateSO[] startingActions;
    
    [Header("Movement")]
    public int moveRange = 4;
 
    [Tooltip("Check each direction this unit is allowed to move toward.")]
    public MovementDirections allowedDirections = MovementDirections.Cardinals;
}
