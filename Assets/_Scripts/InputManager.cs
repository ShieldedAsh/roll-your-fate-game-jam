using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector3 v = Vector3.zero;

    private void Update()
    {
        Debug.Log(v);
    }
    public void OnGyroscopeTilt(InputAction.CallbackContext context)
    {
        Vector3 readDir = context.ReadValue<Vector3>();
        v = readDir;
        Debug.Log(readDir);
        Debug.Log("Beep!");
    }
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