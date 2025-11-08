using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject stickPrefab;
    /// <summary>
    /// Dictionary for sticks: the Object is the key, the collider is the info
    /// </summary>
    private Dictionary<GameObject, BoxCollider2D> stickList;
    
    private GameObject currentStick;
    private Vector3 previousPosition;
    private BoxCollider2D triggerBox; //The box all the sticks are contained in

    private bool running;


    private void Awake()
    {
        stickList = new Dictionary<GameObject, BoxCollider2D>();
        transform.position = Vector3.zero;
        running = true;

        //BOX THAT THE OBJECTS SHALL INHABIT
        triggerBox = gameObject.AddComponent<BoxCollider2D>();
        triggerBox.size = CameraBounds.Instance.screenBounds.size;
        triggerBox.transform.position = Vector3.zero;
        triggerBox.isTrigger = true;

        //BORDER OBJECTS SO STICKS STAY ON SCREEN

        //Left side
        BoxCollider2D leftbox = gameObject.AddComponent<BoxCollider2D>();
        leftbox.size = new Vector2(1, CameraBounds.Instance.screenBounds.size.y);
        leftbox.offset = new Vector2(CameraBounds.Instance.transform.position.x - CameraBounds.Instance.screenBounds.size.x / 2 - leftbox.size.x / 2, 
            CameraBounds.Instance.transform.position.y);

        //Right side
        
        BoxCollider2D rightbox = gameObject.AddComponent<BoxCollider2D>();
        rightbox.size = new Vector2(1, CameraBounds.Instance.screenBounds.size.y);
        rightbox.offset = new Vector2(CameraBounds.Instance.transform.position.x + CameraBounds.Instance.screenBounds.size.x / 2 + rightbox.size.x / 2,
            CameraBounds.Instance.transform.position.y);

        //Bottom
        BoxCollider2D bottombox = gameObject.AddComponent<BoxCollider2D>();
        bottombox.size = new Vector2(CameraBounds.Instance.screenBounds.size.x, 1);
        bottombox.offset = new Vector2(CameraBounds.Instance.transform.position.x,
            CameraBounds.Instance.transform.position.y - CameraBounds.Instance.screenBounds.size.y / 2 - bottombox.size.y / 2);
        
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStick = MakeStick();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (running) //Will be able to be disabled eventually
        {
            Vector3 currentPosition = currentStick.transform.position;

            if (currentPosition == previousPosition)
            {
                currentStick = MakeStick();
            }

            else
            {
                previousPosition = currentPosition;
            }

            Debug.Log(GetPercentCovered() + "%");
        }
    }

    /// <summary>
    /// Makes a stick, adds it an it's collider to the stickList, and returns the gameObject
    /// </summary>
    /// <returns>The GameObject of the stick that was just made</returns>
    private GameObject MakeStick()
    {
        GameObject stick = Instantiate(stickPrefab, new Vector3(Random.Range(-1, 1), 4, 0), Quaternion.identity); //Makes the stick to be spawned
        stick.transform.Rotate(0, 0, Random.Range(-45, 45)); //Rotates the stick between -45 and 45 degrees on the z-axis

        //Randomize the size of the stick
        float stickScale = Random.Range(0.5f, 1.5f);
        stick.transform.localScale *= stickScale;

        stickList.Add(stick, stick.GetComponent<BoxCollider2D>());
        previousPosition = Vector3.zero;
        return stick;
    }

    /// <summary>
    /// Gets the percentage of the screen that's covered, rounded to 2 decimal places
    /// </summary>
    /// <returns>Float representing the percentage of the screen that's covered, rounded to 2 decimal places</returns>
    private float GetPercentCovered()
    {
        float output = 0;
        foreach(GameObject stick in stickList.Keys)
        {
            output += stickList[stick].size.x * stickList[stick].size.y;
        }

        output /= (triggerBox.size.x * triggerBox.size.y);

        output *= 10000;
        output = Mathf.Round(output);
        output /= 100;
        return output;
    }
}
