using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField] GameObject stickPrefab;
    [SerializeField] private GameObject[] stickPrefabs;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float scoreModifier = 2;
    [SerializeField] private int sticksUntilFrozen = 5;
    [SerializeField] private float timeToLoadNextScene = 2;
    /// <summary>
    /// Dictionary for sticks: the Object is the key, the collider is the info
    /// </summary>
    private Dictionary<GameObject, BoxCollider2D> stickDict;

    private InputManager manager;
    public List<GameObject> sticks;
    private GameObject currentStick;
    private Vector3 previousPosition;
    private List<Vector3> fullTrip;
    private bool running;
    private int updates;

    private BoxCollider2D triggerBox; //The box all the sticks are contained in
    private BoxCollider2D leftBox;
    private BoxCollider2D rightBox;
    private BoxCollider2D bottomBox;

    [SerializeField]
    [Tooltip ("Reference to the sound manager.")]
    private GameObject _soundManager;

    private void Awake()
    {
        manager = FindFirstObjectByType<InputManager>();
        //SETS UP DICTIONARY AND BASIC GAMEPLAY ITEMS
        stickDict = new Dictionary<GameObject, BoxCollider2D>();
        transform.position = Vector3.zero;
        running = true;
        fullTrip = new List<Vector3>();
        updates = 0;
        sticks = new List<GameObject>();
        //BOX THAT THE OBJECTS SHALL INHABIT
        triggerBox = gameObject.AddComponent<BoxCollider2D>();
        triggerBox.size = CameraBounds.Instance.screenBounds.size;
        triggerBox.offset = Vector3.zero;
        triggerBox.isTrigger = true;

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
        text.text = $"{GetOpaquePixels():P}";
        if (running)
        {
            Vector3 currentPosition = currentStick.transform.position;
            fullTrip.Add(currentPosition);

            updates++;

            if (updates % 10 == 0 && CheckStopped())
            {
                currentStick = MakeStick();
            }
            else
            {
                previousPosition = currentPosition;
            }
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
        //Vector2 stickScale = stickPrefab.transform.localScale;
        fullTrip = new List<Vector3>();
        //float stickScalar = 0;
        float stickRotate = 0;

        BoxCollider2D spawnBox;
        List<Collider2D> colliders;
        int numCollides;
        //CHECKS FOR COLLISIONS AS THE NEW STICK IS SPAWNED
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"attempt: {i}");
            spawnPosition = new Vector2(Random.Range(-1, 1), 4);
            //stickScalar = Random.Range(0.5f, 1.5f);
            stickRotate = Random.Range(-45, 45);

            spawnBox = gameObject.AddComponent<BoxCollider2D>();
            spawnBox.offset = spawnPosition;
            colliders = new List<Collider2D>();

            Physics2D.OverlapCollider(spawnBox, colliders);
            colliders.Remove(rightBox);
            colliders.Remove(leftBox);
            colliders.Remove(bottomBox);
            colliders.Remove(triggerBox);
            Destroy(spawnBox);

            numCollides = colliders.Count;

            //debug code
            foreach (Collider2D collider in colliders)
            {
                Debug.Log(collider.transform.position.x);
            }
            //POSSIBLE END STATES
            if(numCollides == 0)
            {
                break;
            }
            else if (i == 4)
            {
                running = false;
                GameEnd();
            }
        }

        //MAKES STICK AND RETURNS IT
        if (running)
        {
            //manager.SetOffset();
            GameObject stick = Instantiate(stickPrefabs[Random.Range(0,stickPrefabs.Length)], spawnPosition, Quaternion.identity); //Makes the stick to be spawned
            stick.transform.Rotate(0, 0, stickRotate); //Rotates the stick between -45 and 45 degrees on the z-axis
            //stick.transform.localScale *= stickScalar;

            sticks.Add(stick);
            stickDict.Add(stick, stick.GetComponent<BoxCollider2D>());
            previousPosition = Vector3.zero;
            if (sticks.Count >= sticksUntilFrozen)
            {
                for (int i = 0; i < sticks.Count - sticksUntilFrozen; i++)
                {
                    sticks[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }

            //Plays a sound for when a stick spawns
            _soundManager.GetComponent<SoundManager>().SpawnSound();
            return stick;
        }
        return null;
    }

    private void GameEnd()
    {
        FindFirstObjectByType<ScoreMadoodleThingy>().scorePercent = GetOpaquePixels();
        StartCoroutine(LoadNextScene());
        
    }
    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(timeToLoadNextScene);
        SceneManager.LoadScene(1);
    }
    private float GetOpaquePixels()
    {
        float alphaCutoff = .1f;
        int colorPixels = 0;
        foreach(GameObject obj in sticks)
        {
            Texture2D texture = obj.GetComponent<SpriteRenderer>().sprite.texture;
            Color[] colors = texture.GetPixels(0, 0, texture.width, texture.height);

            foreach(Color c in colors)
                if (c.a > alphaCutoff)
                    colorPixels++;
        }
        float boxPixels = triggerBox.size.x * triggerBox.size.y * 160000 / scoreModifier;
        float stickPercent = colorPixels / (boxPixels * 1.25f);
        return stickPercent;
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

    /// <summary>
    /// Checks if the current stick has stopped moving DOESN'T CURRENTLY WORK!!!
    /// </summary>
    /// <returns>bool of if the stick has stopped moving</returns>
    private bool CheckStopped()
    {
        if (fullTrip.Count < 3)
        {
            return false;
        }

        List<Vector3> positions = new List<Vector3>();

        for(int i = fullTrip.Count - 3; i < fullTrip.Count; i++)
        {
            positions.Add(fullTrip[i]);
        }

        int inRange = 0;

        for(int i = 0; i < positions.Count - 1; i++)
        {
            for(int j = i+1; j < positions.Count; j++)
            {
                if (Vector3.Distance(positions[i], positions[j]) < .005)
                {
                    inRange++;
                }
            }
        }

        if(inRange == 3)
        {
            return true;
        }

        return false;
    }
}