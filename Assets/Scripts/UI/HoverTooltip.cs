using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class HoverTooltip : MonoBehaviour
{
    public static HoverTooltip Instance { get; private set; }
    
    [SerializeField] private GameObject panel;
    
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private Vector2 offset = new Vector2(16f, -16f);
    
    List<GameObject> tooltipsPanel = new List<GameObject>();

    private Dictionary<ITargettable, Tooltip> tooltipsDic = new Dictionary<ITargettable, Tooltip>();
    [SerializeField] private GameObject tooltipPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Transform child in panel.transform)
        {
            tooltipsPanel.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {

    }

    public void Show(HoverableUIData data, Vector2 screenPosition, HoverableUIType hoverType)
    {
        int index = (int)hoverType;
        tooltipsPanel[index].SetActive(true);
        Tooltip tooltip = tooltipsPanel[index].GetComponent<Tooltip>();
        tooltip.nameText.text = data.name;
        tooltip.descriptionText.text = data.description;
        SetPosition(screenPosition);
    }

    public void Hide()
    {
        foreach (GameObject tooltip in tooltipsPanel)
        {
            tooltip.SetActive(false);
        }
    }
    
    
    public void SetPosition(Vector2 screenPosition)
    {
        Vector2 desiredPos = screenPosition + offset;

        float width = tooltipRect.rect.width;
        float height = tooltipRect.rect.height;
        
        float clampedX = Mathf.Clamp(desiredPos.x, 0, Screen.width - width);
        float clampedY = Mathf.Clamp(desiredPos.y, height, Screen.height);

        tooltipRect.position = new Vector2(clampedX, clampedY);
    }

    public void CreateTooltip(ITargettable source, HoverableUIData data, TooltipType type, Vector2 worldPosition)
    {
        if (tooltipsDic.ContainsKey(source))
        {
            return;
        }

        GameObject tooltip = Instantiate<GameObject>(tooltipPrefab);
        tooltip.transform.position =  worldPosition;
        Tooltip tooltipScript = tooltip.GetComponentInChildren<Tooltip>();

        tooltipScript.nameText.text = data.name;
        tooltipScript.descriptionText.text = data.description;

        tooltipsDic.Add(source, tooltipScript);
    }

    public void RemoveTooltip(ITargettable source)
    {
        if (tooltipsDic.TryGetValue(source, out Tooltip toDestroy))
        {
            tooltipsDic.Remove(source);
            Destroy(toDestroy.gameObject);
        }
    }
}
