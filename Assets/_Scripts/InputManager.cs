using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class InputManager : MonoBehaviour
{
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
        Debug.Log(readDir);
    }

    public void OnAttitude(InputAction.CallbackContext context)
    {
        Quaternion readDir = context.ReadValue<Quaternion>();
        Debug.Log(readDir.eulerAngles);
    }
}