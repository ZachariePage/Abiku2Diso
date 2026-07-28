using TMPro;
using UnityEngine;

public enum TooltipType
{
    GridCellInformation
}
public class Tooltip : MonoBehaviour
{
    [SerializeField] public TMP_Text nameText;
    [SerializeField] public TMP_Text descriptionText;
}
