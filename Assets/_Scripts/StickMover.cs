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
        transform.position += new Vector3(Mathf.Clamp(manager.Rotation - 180, -1, 1) * moveSpeed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this);
    }
}