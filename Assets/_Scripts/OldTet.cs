using System;
using TMPro;
using UnityEngine;

public class OldTet : MonoBehaviour
{
    private Gyroscope gyro;
    private Quaternion attitude;
    public TextMeshProUGUI text;
    private void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    private void Update()
    {
        attitude = gyro.attitude;
        text.text = "attitude: " + attitude;
        text.text += $"enabled: {gyro.enabled}";
    }
}
