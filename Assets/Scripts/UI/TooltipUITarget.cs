using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUITarget : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private IHoverable hoverable;

    private void Awake()
    {
        hoverable = GetComponent<IHoverable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverTooltip.Instance.Show(hoverable.GetHoverData(), eventData.position, hoverable.GetHoverType());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverTooltip.Instance.Hide();
    }
}