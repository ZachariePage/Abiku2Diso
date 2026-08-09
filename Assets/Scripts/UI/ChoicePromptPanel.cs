using System.Collections.Generic;
using UnityEngine;

public class ChoicePromptPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private List<ActionOptionButton> slots; 

    private void Awake()
    {
        Hide();
    }

    private void OnEnable()
    {
        BattleLoop.Instance.OnChoicePrompt += Show;
    }

    private void OnDisable()
    {
        BattleLoop.Instance.OnChoicePrompt -= Show;
    }

    private void Show(IReadOnlyList<IActionOption> options)
    {
        if (options.Count > slots.Count)
        {
            Debug.LogWarning($"ChoicePromptPanel has {slots.Count} slots but {options.Count} options were given. Extra options will not be shown.");
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < options.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Setup(options[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        panelRoot.SetActive(true);
    }

    public void HandleOptionClicked(IActionOption option)
    {
        BattleLoop.Instance.SelectPendingOption(option);
        Hide();
    }

    private void Hide()
    {
        panelRoot.SetActive(false);
    }
}
