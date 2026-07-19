#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RetroactionPlayer.RetroactionHookConfig))]
public class RetroactionHookConfigDrawer : PropertyDrawer
{
    private const float Spacing = 2f;
    private const float ComponentDropdownWidth = 110f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty targetProp = property.FindPropertyRelative("target");
        SerializedProperty eventNameProp = property.FindPropertyRelative("eventName");
        SerializedProperty cuesProp = property.FindPropertyRelative("cues");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        Rect targetRect = new Rect(position.x, y, position.width, lineHeight);
        DrawTargetField(targetRect, targetProp);
        y += lineHeight + Spacing;

        Rect eventRect = new Rect(position.x, y, position.width, lineHeight);
        DrawEventDropdown(eventRect, targetProp, eventNameProp);
        y += lineHeight + Spacing;

        float cuesHeight = EditorGUI.GetPropertyHeight(cuesProp, true);
        Rect cuesRect = new Rect(position.x, y, position.width, cuesHeight);
        EditorGUI.PropertyField(cuesRect, cuesProp, new GUIContent("Cues"), true);

        EditorGUI.EndProperty();
    }

    private void DrawTargetField(Rect rect, SerializedProperty targetProp)
    {
        MonoBehaviour currentTarget = targetProp.objectReferenceValue as MonoBehaviour;
        GameObject currentGO = currentTarget != null ? currentTarget.gameObject : null;

        bool hasDropdown = currentGO != null && GetEligibleBehaviours(currentGO).Length > 1;
        float dropdownWidth = hasDropdown ? ComponentDropdownWidth : 0f;
        float gap = hasDropdown ? 4f : 0f;

        Rect objRect = new Rect(rect.x, rect.y, rect.width - dropdownWidth - gap, rect.height);
        Rect dropdownRect = new Rect(objRect.xMax + gap, rect.y, dropdownWidth, rect.height);

        EditorGUI.BeginChangeCheck();
        GameObject newGO = (GameObject)EditorGUI.ObjectField(objRect, "Target", currentGO, typeof(GameObject), true);
        bool goChanged = EditorGUI.EndChangeCheck();

        if (goChanged)
        {
            if (newGO == null)
            {
                targetProp.objectReferenceValue = null;
            }
            else
            {
                var behaviours = GetEligibleBehaviours(newGO);
                if (behaviours.Length == 0)
                {
                    Debug.LogWarning($"No MonoBehaviour found on '{newGO.name}'.");
                    targetProp.objectReferenceValue = null;
                }
                else if (behaviours.Length == 1)
                {
                    targetProp.objectReferenceValue = behaviours[0];
                }
                else
                {
                    targetProp.objectReferenceValue = behaviours[0];
                    ShowComponentPickerMenu(behaviours, targetProp);
                }
            }
            return; 
        }

        if (hasDropdown)
        {
            string buttonLabel = currentTarget != null ? currentTarget.GetType().Name : "-";
            if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(buttonLabel), FocusType.Keyboard))
            {
                ShowComponentPickerMenu(GetEligibleBehaviours(currentGO), targetProp);
            }
        }
    }

    private MonoBehaviour[] GetEligibleBehaviours(GameObject go)
    {
        return go.GetComponents<MonoBehaviour>()
                  .Where(b => b != null)
                  .ToArray();
    }

    private void ShowComponentPickerMenu(MonoBehaviour[] behaviours, SerializedProperty targetProp)
    {
        var so = targetProp.serializedObject;
        string propertyPath = targetProp.propertyPath;

        GenericMenu menu = new GenericMenu();
        foreach (var b in behaviours)
        {
            MonoBehaviour captured = b;
            bool isSelected = targetProp.objectReferenceValue == b;

            menu.AddItem(new GUIContent(b.GetType().Name), isSelected, () =>
            {
                so.Update();
                SerializedProperty prop = so.FindProperty(propertyPath);
                prop.objectReferenceValue = captured;
                so.ApplyModifiedProperties();
            });
        }
        menu.ShowAsContext();
    }


    private void DrawEventDropdown(Rect rect, SerializedProperty targetProp, SerializedProperty eventNameProp)
    {
        MonoBehaviour targetObj = targetProp.objectReferenceValue as MonoBehaviour;

        if (targetObj == null)
        {
            EditorGUI.LabelField(rect, "Event", "(assign a target first)");
            return;
        }

        var eventInfos = GetSupportedEvents(targetObj.GetType());

        if (eventInfos.Count == 0)
        {
            EditorGUI.LabelField(rect, "Event", "(no supported public events found)");
            return;
        }

        var names = eventInfos.Select(e => e.Name).ToArray();
        int currentIndex = Array.IndexOf(names, eventNameProp.stringValue);
        if (currentIndex < 0) currentIndex = 0;

        EditorGUI.BeginChangeCheck();
        int newIndex = EditorGUI.Popup(rect, "Event", currentIndex, names);
        if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(eventNameProp.stringValue))
        {
            eventNameProp.stringValue = names[newIndex];
        }
    }

    private List<EventInfo> GetSupportedEvents(Type type)
    {
        var result = new List<EventInfo>();
        var events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance);

        foreach (var e in events)
        {
            var invoke = e.EventHandlerType.GetMethod("Invoke");
            var parameters = invoke.GetParameters();

            bool supported =
                parameters.Length == 0 ||
                (parameters.Length == 1 && parameters[0].ParameterType == typeof(Vector3));

            if (supported)
                result.Add(e);
        }

        return result.OrderBy(e => e.Name).ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var cuesProp = property.FindPropertyRelative("cues");
        float lineHeight = EditorGUIUtility.singleLineHeight;

        float height = lineHeight + Spacing;
        height += lineHeight + Spacing;
        height += EditorGUI.GetPropertyHeight(cuesProp, true);

        return height;
    }
}
#endif