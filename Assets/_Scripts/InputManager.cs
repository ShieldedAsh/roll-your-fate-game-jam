using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    UnityEngine.Gyroscope gyro;
    private void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        //if (UnityEngine.InputSystem.Gyroscope.current.enabled)
        //    Debug.Log("Gyroscope is enabled");
        //else
        {
            Debug.Log("Gyroscope is not enabled");
            //InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }
    }

    private void Update()
    {
        Debug.Log(gyro.rotationRate);
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