using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> onRightClickEvent;
    public void onRightClick(InputAction.CallbackContext context)
    {
        onRightClickEvent?.Invoke(context);
    }
    
    public static event Action<InputAction.CallbackContext> onLeftClickEvent;
    public void onLeftClick(InputAction.CallbackContext context)
    {
        onLeftClickEvent?.Invoke(context);
    }
    
    public static event Action<InputAction.CallbackContext> onMoveEvent;
    public void onMove(InputAction.CallbackContext context)
    {
        onMoveEvent?.Invoke(context);
    }
    
    public static event Action<InputAction.CallbackContext> onMouseMoveEvent;
    public void onMouseMove(InputAction.CallbackContext context)
    {
        onMouseMoveEvent?.Invoke(context);
    }
}
