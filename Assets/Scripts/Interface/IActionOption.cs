using System.Collections.Generic;
using UnityEngine;

public interface IActionOption
{
    string Id { get; }
    string Label { get; }
    Sprite Icon { get; }
}

public interface IActionOption<out T> : IActionOption
{
    T GetValue();
}

public readonly struct ActionOption<T> : IActionOption<T>
{
    public T GetValue()
    {
        return Value;
    }

    public string Id { get; }
    public string Label { get; }
    public Sprite Icon { get; }
    
    public readonly T Value;
    
    public ActionOption(string id, string label, T value, Sprite icon = null)
    {
        Id = id;
        Label = label;
        Value = value;
        Icon = icon;
    }
}

public interface IChoiceGatedAction
{
    IReadOnlyList<IActionOption> GetOptions();
    bool SelectOption(IActionOption option);
    bool HasPendingChoice();

    bool StartActionImmediately();
}
