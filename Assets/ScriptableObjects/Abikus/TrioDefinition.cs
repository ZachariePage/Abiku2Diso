using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Trio")]
public class TrioDefinition : ScriptableObject
{
    public GameObject prefab;
    public AbilityTemplateSO[] startingActions;

    public int HP;
    public int moveRange;

    public int defence;
    
    public MovementDirections moveDirection;
    //public Abiku[] startingAbiku = new Abiku[3];
}
