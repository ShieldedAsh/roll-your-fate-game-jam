using Unity.VisualScripting;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private static CameraBounds instance;
    public static CameraBounds Instance { get { return GetInstance(); } }
    private Vector3 screenBottomLeft;
    public Bounds screenBounds;

    private static CameraBounds GetInstance()
    {
        if (instance == null)
        {
            instance = Camera.main.GetComponent<CameraBounds>();
            if (instance == null)
                instance = Camera.main.AddComponent<CameraBounds>();
        }
        return instance;
    }

    private void Awake()
    {
        GetScreenBounds();
    }
    /// <summary>
    /// gets the Main camera's screen bounds
    /// </summary>
    private void GetScreenBounds()
    {
        screenBottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero) * 2;
        screenBounds = new Bounds(Camera.main.transform.position, (-screenBottomLeft) - Camera.main.transform.position);
    }
    private void OnDrawGizmosSelected()
    {
        GetScreenBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(screenBounds.center, screenBounds.size);
    }

    /// <summary>
    /// checks if a collider is being rendered
    /// </summary>
    /// <param name="other">the collider to check against</param>
    /// <returns>true if the collider is being rendered by Camera.Main, otherwise false</returns>
    public bool IsRendered(Collider2D other)
    {
        return screenBounds.Intersects(other.bounds);
    }
}