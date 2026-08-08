using UnityEngine;

public class Health : MonoBehaviour
{
    private GridActor actor;
    [SerializeField]
    private float health;
    [SerializeField]
    private float StartingHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GridActor actor, float health)
    {
        this.actor = actor;
        this.health = health;
        this.StartingHealth = health;
    }

    public void SetHP(float hp)
    {
        health = hp;
    }

    public float GetHP()
    {
        return health;
    }

    public void ModifyHp(float hp)
    {
        health += hp;
    }
    public bool IsWounded()
    {
        return health < StartingHealth;
    }
}
