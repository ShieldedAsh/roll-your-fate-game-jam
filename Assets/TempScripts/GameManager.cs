using System.Collections.Generic;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject stickPrefab;
    /// <summary>
    /// Dictionary for sticks: the Object is the key, the collider is the info
    /// </summary>
    private Dictionary<GameObject, BoxCollider2D> stickDict;
    
    private GameObject currentStick;
    private Vector3 previousPosition;
    private BoxCollider2D triggerBox; //The box all the sticks are contained in

    private bool running;
    BoxCollider2D leftBox;
    BoxCollider2D rightBox;
    BoxCollider2D bottomBox;

    private void Awake()
    {
        stickDict = new Dictionary<GameObject, BoxCollider2D>();
        transform.position = Vector3.zero;
        running = true;

        //BOX THAT THE OBJECTS SHALL INHABIT
        triggerBox = gameObject.AddComponent<BoxCollider2D>();
        triggerBox.size = CameraBounds.Instance.screenBounds.size;
        triggerBox.offset = Vector3.zero;
        triggerBox.isTrigger = true;
        //triggerBox.tag = "triggerBox";

        //BORDER OBJECTS SO STICKS STAY ON SCREEN

        //Left side
        leftBox = gameObject.AddComponent<BoxCollider2D>();
        leftBox.size = new Vector2(1, CameraBounds.Instance.screenBounds.size.y);
        leftBox.offset = new Vector2(CameraBounds.Instance.transform.position.x - CameraBounds.Instance.screenBounds.size.x / 2 - leftBox.size.x / 2, 
            CameraBounds.Instance.transform.position.y);

        //Right side
        rightBox = gameObject.AddComponent<BoxCollider2D>();
        rightBox.size = new Vector2(1, CameraBounds.Instance.screenBounds.size.y);
        rightBox.offset = new Vector2(CameraBounds.Instance.transform.position.x + CameraBounds.Instance.screenBounds.size.x / 2 + rightBox.size.x / 2,
            CameraBounds.Instance.transform.position.y);

        //Bottom
        bottomBox = gameObject.AddComponent<BoxCollider2D>();
        bottomBox.size = new Vector2(CameraBounds.Instance.screenBounds.size.x, 1);
        bottomBox.offset = new Vector2(CameraBounds.Instance.transform.position.x,
            CameraBounds.Instance.transform.position.y - CameraBounds.Instance.screenBounds.size.y / 2 - bottomBox.size.y / 2);
        
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStick = MakeStick();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (running)
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
    /// Checks if a stick can be spawned, making 100 attempts (5 FOR DEBUGGING).
    /// If possible: Makes a stick, adds it an it's collider to the stickDict, and returns the stick's GameObject
    /// If not: Stops the game from running anymore and returns null
    /// </summary>
    /// <returns>The GameObject of the stick that was just made or null if none was made</returns>
    private GameObject MakeStick()
    {
        Debug.Log("making a stick");
        Vector2 spawnPosition = Vector2.zero;
        Vector2 stickScale = stickPrefab.transform.localScale;
        float stickScalar = 0;
        float stickRotate = 0;

        Collider2D spawnBox;
        List<Collider2D> colliders;
        int numCollides;

        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"attempt: {i}");
            spawnPosition = new Vector2(Random.Range(-1, 1), 4);
            stickScalar = Random.Range(0.5f, 1.5f);
            stickRotate = Random.Range(-45, 45);

            spawnBox = Physics2D.OverlapBox(Vector2.zero, stickScale * stickScalar, stickRotate);
            spawnBox.offset = spawnPosition; //MOVING TRIGGERBOX?????
            colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(spawnBox, colliders);
            colliders.Remove(rightBox);
            colliders.Remove(leftBox);
            colliders.Remove(bottomBox);

            numCollides = colliders.Count;

            Debug.Log("Colliding with:");
            foreach (Collider2D collider in colliders)
            {
                Debug.Log(collider.transform.position.x);
            }
            if(numCollides == 0)
            {
                Debug.Log("success");
                break;
            }
            else if (i == 4)
            {
                running = false;
                Debug.Log("GAME STOPPED");
            }
            Debug.Log("fail");
        }

        if (running)
        {
            GameObject stick = Instantiate(stickPrefab, spawnPosition, Quaternion.identity); //Makes the stick to be spawned
            stick.transform.Rotate(0, 0, stickRotate); //Rotates the stick between -45 and 45 degrees on the z-axis
            stick.transform.localScale *= stickScalar;


            stickDict.Add(stick, stick.GetComponent<BoxCollider2D>());
            previousPosition = Vector3.zero;

            return stick;
        }
        return null;
    }

    /// <summary>
    /// Gets the percentage of the screen that's covered, rounded to 2 decimal places
    /// </summary>
    /// <returns>Float representing the percentage of the screen that's covered, rounded to 2 decimal places</returns>
    private float GetPercentCovered()
    {
        float output = 0;
        foreach(GameObject stick in stickDict.Keys)
        {
            output += stickDict[stick].size.x * stickDict[stick].size.y;
        }

        output /= (triggerBox.size.x * triggerBox.size.y);

        output *= 10000;
        output = Mathf.Round(output);
        output /= 100;
        return output;
    }
}
