using System.Collections.Generic;
using UnityEngine;

public class BoulderManager
{
    private ShieldBreaker _shieldBreaker;
    private ShieldBreakerEgungunDefinition _config;
    private int _maxLight;
    private int _maxHeavy;

    private List<LightBoulder> _lightBoulders = new();
    private List<HeavyBoulder> _heavyBoulders = new();
    
    public BoulderManager(ShieldBreaker shieldBreaker,ShieldBreakerEgungunDefinition config)
    {
        this._shieldBreaker = shieldBreaker;
        _config = config;
        _maxLight = config.maxLightBoulders;
        _maxHeavy = config.maxHeavyBoulders;
    }
    
    public Boulder Summon(BoulderType type, GridCell cell)
    {
        Boulder boulder = null;
        switch (type)
        {
            case BoulderType.light:
                LightBoulder lightBoulder = UnitSpawner.Instance.SpawnActor(_config.lightBoulderPrefab, cell).GetComponent<LightBoulder>();
                boulder = lightBoulder;
                _lightBoulders.Add(lightBoulder);
                break;
            case BoulderType.heavy:
                HeavyBoulder heavyBoulder = UnitSpawner.Instance.SpawnActor(_config.heavyBoulderPrefab, cell).GetComponent<HeavyBoulder>();
                boulder = heavyBoulder;
                _heavyBoulders.Add(heavyBoulder);
                break;
        }
        
        return boulder;
    }

    public bool CanSpawnTypeOfBoulder(BoulderType type)
    {
        switch (type)
        {
            case BoulderType.light:
                if (_lightBoulders.Count >= _maxLight)
                {
                    return false;
                }
                break;
            case BoulderType.heavy:
                if (_heavyBoulders.Count >= _maxHeavy)
                {
                    return false;
                }
                break;
            default:
                return false;
        }
        return true;
    }
}
