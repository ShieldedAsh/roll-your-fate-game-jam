using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class InputManager : MonoBehaviour
{
    private Vector3 angularVelocity;
    private Quaternion rotation;
    private void Start()
    {
        if(Gyroscope.current != null)
        {
            InputSystem.EnableDevice(Gyroscope.current);
        }
    }
    public void OnVelocityChanged(InputAction.CallbackContext context)
    {
        angularVelocity = context.ReadValue<Vector3>();
        Debug.Log($"angular velocity: {angularVelocity}");
    }
    public void OnRotationChanged(InputAction.CallbackContext context)
    {
        rotation = context.ReadValue<Quaternion>();
        Debug.Log($"rotation: {rotation}");
    }
}