using UnityEngine;

public class LightBoulder : Boulder
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void OnPushed(BoulderManager manager, GridActor pusher, Vector2Int direction)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
