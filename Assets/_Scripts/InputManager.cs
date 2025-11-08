using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class InputManager : MonoBehaviour
{
    private Vector3 angularVelocity;
    private Quaternion rotation;
    private bool clicked;

    public TextMeshProUGUI text;
    private void Start()
    {
        if(Gyroscope.current != null)
        {
            InputSystem.EnableDevice(Gyroscope.current);
        }
    }

    private void Update()
    {
        text.text = $"angularVelocity: {angularVelocity}\n" +
                    $"rotation: {rotation}\n" +
                    $"1 finger press test: {clicked}\n";
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

    public void OnTest(InputAction.CallbackContext context)
    {
        clicked = context.ReadValue<bool>();
    }
}