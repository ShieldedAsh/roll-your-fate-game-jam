using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class InputManager : MonoBehaviour
{
    private void Start()
    {
        InputSystem.EnableDevice(Gyroscope.current);
        if (Gyroscope.current.enabled)
            Debug.Log("Gyroscope is enabled");
        else
            Debug.Log("Gyroscope is not enabled");
    }
    public void OnGyroscopeTilt(InputAction.CallbackContext context)
    {
        Vector3 readDir = context.ReadValue<Vector3>();
        Debug.Log(readDir);
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