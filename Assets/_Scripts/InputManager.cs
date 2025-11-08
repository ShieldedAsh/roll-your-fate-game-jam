using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnTilt(InputAction.CallbackContext context)
    {
        Vector2 readDir = context.ReadValue<Vector2>();
        Debug.Log(readDir);
    }
    public void OnTwist(InputAction.CallbackContext context)
    {
        float readDir = context.ReadValue<float>();
    }
}