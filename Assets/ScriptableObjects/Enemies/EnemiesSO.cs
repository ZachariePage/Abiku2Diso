using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Enemie")]
public class EnemiesSO : ScriptableObject
{
    public GameObject prefab;
    public float baseHealth;
    public float baseDamage;
    public AbilityTemplateSO[] startingActions;
}
