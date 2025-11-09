using UnityEngine;

public class StickMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private InputManager manager;
    private void Awake()
    {
        manager = FindFirstObjectByType<InputManager>();
    }

    private void Update()
    {
        transform.position += new Vector3(manager.Rotation * moveSpeed * Time.deltaTime, 0, 0);
    }
}