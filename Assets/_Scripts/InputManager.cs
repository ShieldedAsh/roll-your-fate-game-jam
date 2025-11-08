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

    public Quaternion attitudeOffset;
    [Tooltip("The current rotation velocity of the device")]
    public Vector3 rotationRate;
    [Tooltip("The current acceleration of the device")]
    public Vector3 acceleration;
}
public class InputManager : MonoBehaviour
{
    [Header("functions")]
    public float flickAcclerationRate;
    [Header("Debug stuff")]
    [Tooltip("if checked, displays the gyro's stats")]
    public bool DebugEnabled;
    public TextMeshProUGUI debugText;
    public Transform testCube;

    private Gyroscope gyro;
    private static GyroscopeData data;
    [Tooltip("the current gyroscope's data")]
    public static GyroscopeData CurrentGyroscope { get => data; }

    public Quaternion OffsetAttitude { get { return Quaternion.Euler(CurrentGyroscope.attitude.eulerAngles - CurrentGyroscope.attitudeOffset.eulerAngles); } }
    private void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        SetOffset();
    }

    private void Update()
    {
        //perspective is flipped in GetInput() rather than before to reduce stupidity of us -vk
        GetInput();
        testCube.transform.rotation = OffsetAttitude;

        if (debugText)
            Display();
    }

    public void SetOffset()
    {
        data.attitudeOffset = GyroFlipper(CurrentGyroscope.attitude);
    }

    private void GetInput()
    {
        data.attitude = GyroFlipper(gyro.attitude);
        data.rotationRate = gyro.rotationRateUnbiased;
        data.acceleration = gyro.userAcceleration;
    }
    private void Display()
    {
        debugText.text = $"attitude: {Mathf.Round(OffsetAttitude.eulerAngles.x * 100) / 100},\t{Mathf.Round(OffsetAttitude.eulerAngles.y * 100) / 100},\t{Mathf.Round(OffsetAttitude.eulerAngles.z * 100) / 100}\n" +
                         $"rotationRate: {Mathf.Round(data.rotationRate.x * 100) / 100},\t{Mathf.Round(data.rotationRate.y * 100) / 100},\t{Mathf.Round(data.rotationRate.z * 100) / 100}\n" +
                         $"accelerationRate: {Mathf.Round(data.acceleration.x * 100) / 100},\t{Mathf.Round(data.acceleration.y * 100) / 100},\t{Mathf.Round(data.acceleration.z * 100) / 100}";
    }

    private Quaternion GyroFlipper(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}