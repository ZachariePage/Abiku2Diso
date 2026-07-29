using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleCommand : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    void Start()
    {
        InputListener.onConsoleCommandPressed += OnConsoleCommandButtonPressed;
    }

    public void OnConsoleCommandButtonPressed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_inputField.gameObject.activeSelf)
        {
            _inputField.gameObject.SetActive(false);
        }
        else
        {
            _inputField.gameObject.SetActive(true);
        }
        
    }

    // Hook this to the TMP_InputField's "On Submit" event too, so pressing Enter works.
    public void SubmitCommand()
    {
        if (_inputField == null || string.IsNullOrWhiteSpace(_inputField.text))
            return;

        string input = _inputField.text.Trim();
        Debug.Log($"[Console] > {input}");

        CheatManager.Instance.ExecuteCommand(input);

        _inputField.text = string.Empty;
        _inputField.ActivateInputField();
    }
}