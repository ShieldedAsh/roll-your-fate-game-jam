using TMPro;
using UnityEngine;

[Tooltip("data read by a gyroscope")]
public struct GyroscopeData
{
    //z=rotate along z
    //x=rotate along y
    //y=rotate along x
    [Tooltip("The current angle of the device")]
    public Quaternion attitude;

    [Tooltip("the attitude that makes the phone make sense")]
    public Vector3 PlayerAttitude { get { return new Vector3(attitude.eulerAngles.x, attitude.eulerAngles.y, attitude.eulerAngles.z); } }
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
        testCube.transform.rotation = Quaternion.Euler(CurrentGyroscope.PlayerAttitude);
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
        debugText.text = $"attitude: {Mathf.Round(data.attitude.eulerAngles.x * 100) / 100}, {Mathf.Round(data.attitude.eulerAngles.y * 100) / 100}, {Mathf.Round(data.attitude.eulerAngles.z * 100) / 100}\n" +
                         $"rotationRate: {Mathf.Round(data.rotationRate.x * 100) / 100}, {Mathf.Round(data.rotationRate.y * 100) / 100}, {Mathf.Round(data.rotationRate.z * 100) / 100}\n" +
                         $"accelerationRate: {Mathf.Round(data.acceleration.x * 100) / 100}, {Mathf.Round(data.acceleration.y * 100) / 100}, {Mathf.Round(data.acceleration.z * 100) / 100}";
    }
}