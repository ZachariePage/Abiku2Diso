using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance { get; private set; }
    
    public List<ActorEffectDefinition> actorEffectsDefinition = new List<ActorEffectDefinition>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ExecuteCommand(string rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput)) return;

        string[] parts = rawInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToLowerInvariant();
        string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

        switch (command)
        {
            case "damage":
            case "unittakedamage":
                if (args.Length < 1 || !int.TryParse(args[0], out int damage))
                {
                    Debug.LogWarning("Usage: damage <amount>");
                    return;
                }
                UnitTakeDamage(damage);
                break;
            case "actoreffectdefinition":
                if (args.Length < 1)
                {
                    Debug.LogWarning("Usage: actoreffectdefinition <EffectName>");
                    return;
                }

                string effectName = args[0];
                CreateEffect(effectName);
                break;
            case "resetturn":
                foreach (AbikuTrio abikuTrio in FindObjectsByType<AbikuTrio>(FindObjectsSortMode.None))
                {
                    abikuTrio.Cheat_refreshcoldown();
                }
                BattleLoop.Instance.Cheat_SetbattlePhase(BattlePhase.Combat);
                break;
            default:
                Debug.LogWarning($"Unknown command: '{command}'");
                break;
        }
    }

    public void UnitTakeDamage(int damage)
    {
        ITargettable target = BattleLoop.Instance.GetSelectedTarget();
        if (target == null)
        {
            Debug.LogWarning("Selected target is null");
            return;
        }

        GridActor actor = target.GetActor();
        if (actor == null)
        {
            Debug.LogWarning("Actor is null");
            return;
        }

        IDamageable damageable = actor.GetComponent<IDamageable>();
        if (damageable == null)
        {
            Debug.LogWarning("Damageable is null");
            return;
        }

        damageable.TakeDamage(null, null, damage, Element.None);
    }

    public void CreateEffect(string effectName)
    {
        ITargettable target = BattleLoop.Instance.GetSelectedTarget();
        if (target == null)
        {
            Debug.LogWarning("Selected target is null");
            return;
        }

        GridActor actor = target.GetActor();
        if (actor == null)
        {
            Debug.LogWarning("Actor is null");
            return;
        }
        
        ActorEffectDefinition effectDefinition = actorEffectsDefinition.Find(
            effect => effect.EffectName.Equals(effectName, StringComparison.OrdinalIgnoreCase));

        if (effectDefinition == null)
        {
            Debug.LogWarning($"ActorEffectDefinition '{effectName}' not found.");
            return;
        }

        ActorEffect effect = effectDefinition.CreateEffect();

        if (actor is AbikuTrio trio)
        {
            trio.GetComponent<ActorEffectManager>().AddEffect(effect, actor);
        }
    }
}