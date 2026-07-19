using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

public class RetroactionPlayer : MonoBehaviour
{
    [Serializable]
    public class RetroactionHookConfig
    {
        [Tooltip("Just drag and drop the gameobject, then pick the correct script")]
        public MonoBehaviour target;

        [Tooltip("Pick which event you want the cues to fire on")]
        public string eventName;

        [Tooltip("cues that will be played for event")]
        public List<GameCue> cues;

        [NonSerialized] public Delegate boundHandler;
    }
    
    [SerializeField] private List<RetroactionHookConfig> Retroactions = new List<RetroactionHookConfig>();

    private void OnEnable()
    {
        foreach (var hook in Retroactions)
        {
            Bind(hook);
        }
    }

    private void OnDisable()
    {
        foreach (var hook in Retroactions)
        {
            Unbind(hook);
        }
    }

    private void Bind(RetroactionHookConfig hook)
    {
        if (hook.target == null || string.IsNullOrEmpty(hook.eventName)) return;

        EventInfo eventInfo = GetEvent(hook);
        if (eventInfo == null)
        {
            Debug.LogWarning($"[RetroactionPlayer] no event '{hook.eventName}' found on {hook.target.GetType().Name}", this);
            return;
        }

        ParameterInfo[] invokeParams = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters();
        Delegate del;

        if (invokeParams.Length == 0)
        {
            Action action = delegate
            {
                PlayCues(hook, transform.position);
            };

            del = action;
        }
        else if (invokeParams.Length == 1 && invokeParams[0].ParameterType == typeof(Vector3))
        {
            Action<Vector3> action = delegate (Vector3 position)
            {
                PlayCues(hook, position);
            };

            del = action;
        }
        else
        {
            string message = "[RetroactionPlayer] Unsupported event signature for '" + hook.eventName + "' "
                             + "(" + invokeParams.Length + " params). Add a case for it in RetroactionPlayer.Bind().";

            Debug.LogWarning(message, this);
            return;
        }

        eventInfo.AddEventHandler(hook.target, del);
        hook.boundHandler = del;
    }

    private void Unbind(RetroactionHookConfig hook)
    {
        if (hook.target == null || hook.boundHandler == null)
            return;

        var eventInfo = GetEvent(hook);
        eventInfo?.RemoveEventHandler(hook.target, hook.boundHandler);
        hook.boundHandler = null;
    }

    private EventInfo GetEvent(RetroactionHookConfig hook)
    {
        return hook.target.GetType().GetEvent(hook.eventName, BindingFlags.Public | BindingFlags.Instance);
    }

    private void PlayCues(RetroactionHookConfig hook, Vector3 position)
    {
        if (hook.cues == null) return;

        foreach (var cue in hook.cues)
        {
            cue?.Execute(position);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var hook in Retroactions)
        {
            if (hook.target == null || string.IsNullOrEmpty(hook.eventName))
                continue;

            if (GetEvent(hook) == null)
                Debug.LogWarning($"[RetroactionPlayer] '{hook.eventName}' is not a public event on {hook.target.GetType().Name}.", this);
        }
    }
#endif
}