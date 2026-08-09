using System;
using UnityEngine;

[CreateAssetMenu(menuName = "VisualUI/ActionOptionsTemplate")]
public class ActionOptionsTemplate : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
}

[Serializable]
public class StanceOption
{
    public ElementSO stance;
    public ActionOptionsTemplate visual;
}