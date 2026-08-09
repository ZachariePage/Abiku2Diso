using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionOptionButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button button;

    private IActionOption boundOption;
    private ChoicePromptPanel choicePromptPanel;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        choicePromptPanel = GetComponentInParent<ChoicePromptPanel>();
    }
    
    public void Setup(IActionOption option)
    {
        boundOption = option;

        icon.sprite = option.Icon;
        label.text = option.Label;
    }

    public void OnClick()
    {
        choicePromptPanel.HandleOptionClicked(boundOption);
    }
}