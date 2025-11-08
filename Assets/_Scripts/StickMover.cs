using UnityEngine;

public class StickMover : MonoBehaviour
{
    public float moveSpeed;

    private InputManager manager;
    private void Awake()
    {
        manager = FindFirstObjectByType<InputManager>();
    }

    private void Update()
    {
        transform.position += transform.position + new Vector3(manager.Rotation * moveSpeed, 0, 0);
    }
}