using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStats : MonoBehaviour
{
    private Dictionary<Type, List<Delegate>> listeners = new();
    public static BattleStats Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Subscribe<T>(Action<T> callback)
        where T : IBattleEvent
    {
        Type type = typeof(T);

        if (!listeners.ContainsKey(type))
        {
            listeners[type] = new List<Delegate>();
        }

        listeners[type].Add(callback);
    }
    
    public void Unsubscribe<T>(Action<T> callback) 
        where T : IBattleEvent
    {
        if(listeners.TryGetValue(typeof(T), out var list))
        {
            list.Remove(callback);
        }
    }


    public void Broadcast<T>(T battleEvent) where T : IBattleEvent
    {
        Type type = typeof(T);

        if (!listeners.TryGetValue(type, out var callbacks))
            return;

        var snapshot = callbacks.ToArray();
        foreach (var callback in snapshot)
        {
            ((Action<T>)callback)(battleEvent);
        }
    }
}
