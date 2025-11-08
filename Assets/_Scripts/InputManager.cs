using TMPro;
using UnityEngine;

[Tooltip("data read by a gyroscope")]
public struct GyroscopeData
{
    [Tooltip("The current angle of the device")]
    public Quaternion attitude;
    [Tooltip("The current rotation velocity of the device")]
    public Vector3 rotationRate;
    [Tooltip("The current acceleration of the device")]
    public Vector3 acceleration;
}
public class InputManager : MonoBehaviour
{
    [Header("functions")]
    public float flicAcclerationRate;
    [Header("Debug stuff")]
    [Tooltip("if checked, displays the gyro's stats")]
    public bool DebugEnabled;
    public TextMeshProUGUI debugText;
    public Transform testCube;

    private Gyroscope gyro;
    private static GyroscopeData data;
    [Tooltip("the current gyroscope's data")]
    public static GyroscopeData CurrentGyroscope { get => data; }
    private void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    private void Update()
    {
        testCube.transform.rotation = CurrentGyroscope.attitude;
        testCube.GetComponent<Rigidbody2D>().linearVelocity = CurrentGyroscope.rotationRate;
        GetInput();

        if (debugText)
            Display();
    }

    private void GetInput()
    {
        data.attitude = gyro.attitude;
        data.rotationRate = gyro.rotationRateUnbiased;
        data.acceleration = gyro.userAcceleration;
    }
    private void Display()
    {
        debugText.text = $"attitude: {data.attitude}\n" +
                         $"rotationRate: {data.rotationRate}\n" +
                         $"accelerationRate: {data.acceleration}";
    }
}