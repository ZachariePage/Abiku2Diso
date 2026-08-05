using TMPro;
using UnityEngine;

public class UITextBattlePhase : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = BattleLoop.Instance.CurrentPhase.ToString();
    }
}
