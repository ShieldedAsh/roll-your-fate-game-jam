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

public enum InputData
{
    None,
    FlickLeft,
    FlickRight,
    FlickTo,
    FlickAway
}
public class InputManager : MonoBehaviour
{
    [Header("functions")]
    [Tooltip("how many seconds should we not read inputs for after one is read")]
    public float pollingDelay;
    [Tooltip("how fast you have to flick the phone for it to be read as an input")]
    public float flickAcclerationRate;
    [Tooltip("the currently read flick direction")]
    private InputData inputData = InputData.None;
    [Tooltip("the current rotation of the phone")]
    private float rotation;
    [Tooltip("The current flick direction")]
    public InputData FlickDirection { get => inputData; }
    [Tooltip("the current phone rotation")]
    public float GyroRotation { get => rotation; }
    [Header("Debug stuff")]
    [Tooltip("if checked, displays the gyro's stats")]
    public bool DebugEnabled;
    [Tooltip("the debug text to be displayed")]
    public TextMeshProUGUI debugText;
    [Tooltip("the test cube")]
    public Transform testCube;

    private Gyroscope gyro;
    private static GyroscopeData data;
    [Tooltip("the current gyroscope's data")]
    public static GyroscopeData CurrentGyroscope { get => data; }

    public Quaternion OffsetAttitude { get { return Quaternion.Euler(CurrentGyroscope.attitude.eulerAngles - CurrentGyroscope.attitudeOffset.eulerAngles); } }

    private float timer;
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
        ProcessData();
        testCube.transform.rotation = OffsetAttitude;

        if (debugText)
            Display();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            SetOffset();
    }

    public void SetOffset()
    {
        data.attitudeOffset = CurrentGyroscope.attitude;
    }

    private void GetInput()
    {
        data.attitude = gyro.attitude;
        data.rotationRate = gyro.rotationRateUnbiased;
        data.acceleration = gyro.userAcceleration;
    }

    private void ProcessData()
    {
        rotation = OffsetAttitude.y;

        if(inputData != InputData.None)
        {
            timer = 0;
            Debug.Log(inputData.ToString());
            inputData = InputData.None;
        }
        if(timer >= pollingDelay)
        {
            Vector3 accleerationRate = CurrentGyroscope.acceleration;
            if (accleerationRate.x >= flickAcclerationRate)
                inputData = InputData.FlickRight;
            else if (accleerationRate.x <= -flickAcclerationRate)
                inputData = InputData.FlickLeft;
            else if (accleerationRate.y >= flickAcclerationRate)
                inputData = InputData.FlickAway; //away - maybe doesnt work
            else if (accleerationRate.y <= -flickAcclerationRate)
                inputData = InputData.FlickTo; //away - maybe doesnt work
        }
        else
        {
            timer += Time.deltaTime;
        }

    }
    private void Display()
    {
        debugText.text = $"attitude: {Mathf.Round(OffsetAttitude.eulerAngles.x * 100) / 100}, {Mathf.Round(OffsetAttitude.eulerAngles.y * 100) / 100}, {Mathf.Round(OffsetAttitude.eulerAngles.z * 100) / 100}\n" +
                         $"rotationRate: {Mathf.Round(data.rotationRate.x * 100) / 100}, {Mathf.Round(data.rotationRate.y * 100) / 100}, {Mathf.Round(data.rotationRate.z * 100) / 100}\n" +
                         $"accelerationRate: {Mathf.Round(data.acceleration.x * 100) / 100}, {Mathf.Round(data.acceleration.y * 100) / 100}, {Mathf.Round(data.acceleration.z * 100) / 100}\n" +
                         $"rotation: {rotation}\n" +
                         $"acceleration: {inputData.ToString()}";
    }
}