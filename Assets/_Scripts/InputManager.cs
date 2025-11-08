using TMPro;
using UnityEngine;

/// <summary>
/// all the data read by the gyroscope
/// </summary>
public struct GyroscopeData
{
    //z=rotate along z
    //x=rotate along y
    //y=rotate along x
    [Tooltip("The current angle of the device")]
    public Quaternion attitude;
    [Tooltip("the current offset for the attitude")]
    public Quaternion attitudeOffset;
    [Tooltip("The current rotation velocity of the device")]
    public Vector3 rotationRate;
    [Tooltip("The current acceleration of the device")]
    public Vector3 acceleration;

    public Quaternion ModifiedAttitude { get => Quaternion.Euler(attitude.eulerAngles - attitudeOffset.eulerAngles); }
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
    [SerializeField]private float pollingDelay;

    [Tooltip("how fast you have to flick the phone for it to be read as an input")]
    [SerializeField]private float flickAcclerationRate;

    [Tooltip("the currently read flick direction")]
    private InputData inputData = InputData.None;

    [Tooltip("the current rotation of the phone")]
    private float rotation;

    public float Rotation { get => rotation; }
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
    [SerializeField]private Transform testCube;


    /// ---gyroscope variables---
    private Gyroscope gyro;
    private static GyroscopeData data;
    [Tooltip("the current gyroscope's data")]
    public static GyroscopeData CurrentGyroscope { get => data; }

    private float timer;

    /// <summary>
    /// gets the current gyroscope and gets a starting offset so that things rotate correctly
    /// </summary>
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

        //if you tap the screen reset the offsets
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            SetOffset();

        //debug display to make sure everything is working
        if (DebugEnabled)
            DebugInputs();
    }


    /// <summary>
    /// gets an offset for the attitude sensor
    /// </summary>
    public void SetOffset()
    {
        data.attitudeOffset = CurrentGyroscope.attitude;
    }

    /// <summary>
    /// reads all the data from the sensors
    /// </summary>
    private void GetInput()
    {
        data.attitude = gyro.attitude;
        data.rotationRate = gyro.rotationRateUnbiased;
        data.acceleration = gyro.userAcceleration;
    }


    private InputData LastReadInput;

    /// <summary>
    /// processes the sensor data to be used by other scripts
    /// </summary>
    private void ProcessData()
    {
        //gets the rotation cause we always need that
        rotation = CurrentGyroscope.ModifiedAttitude.eulerAngles.y;

        //there is a delay after an input is read where it doesn't update so we can make sure that the input was read intenionally
        if(inputData != InputData.None)
        {
            timer = 0;
            LastReadInput = inputData;
            Debug.Log(inputData.ToString());
            inputData = InputData.None;
        }
        if(timer >= pollingDelay)
        {
            Vector3 accleerationRate = CurrentGyroscope.acceleration;
            if (accleerationRate.x >= flickAcclerationRate)
                inputData = InputData.FlickLeft;
            else if (accleerationRate.x <= -flickAcclerationRate)
                inputData = InputData.FlickRight;
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

    /// <summary>
    /// debug display all the functions this code does
    /// </summary>
    private void DebugInputs()
    {
        debugText.text = $"attitude: {(Mathf.Round(CurrentGyroscope.ModifiedAttitude.eulerAngles.x * 100) / 100):F2}, {(Mathf.Round(CurrentGyroscope.ModifiedAttitude.eulerAngles.y * 100) / 100):F2}, {(Mathf.Round(CurrentGyroscope.ModifiedAttitude.eulerAngles.z * 100) / 100):F2}\n" +
                         $"rotationRate: {(Mathf.Round(data.rotationRate.x * 100) / 100):F2}, {Mathf.Round(data.rotationRate.y * 100) / 100:F2}, {Mathf.Round(data.rotationRate.z * 100) / 100:F2}\n" +
                         $"accelerationRate: {Mathf.Round(data.acceleration.x * 100) / 100:F2}, {Mathf.Round(data.acceleration.y * 100) / 100:F2}, {Mathf.Round(data.acceleration.z * 100) / 100:F2}\n" +
                         $"rotation: {rotation:F2}\n" +
                         $"acceleration: {LastReadInput.ToString()}";
        testCube.transform.rotation = CurrentGyroscope.ModifiedAttitude;
    }
}